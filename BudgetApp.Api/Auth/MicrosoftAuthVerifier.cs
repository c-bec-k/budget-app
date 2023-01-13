using System;
using Newtonsoft.Json.Linq;

namespace BudgetApp.Api.Auth;

public class MicrosoftAuthVerifier<T>
{
  private readonly MSAuthSettings _microsoftAuthSettings;
  private readonly string _host;
  private readonly ILogger<T> _logger;


  public MicrosoftAuthVerifier(MSAuthSettings microsoftAuthSettings, string host, ILogger<T> logger)
  {
    _microsoftAuthSettings = microsoftAuthSettings;
    _host = host;
    _logger = logger;
  }

  public async Task<MicrosoftUserProfile> AcquireUser(string token)
  {
    try
    {
      var client = new HttpClient();

      var tokenRequestParameters = new Dictionary<string, string>()
      {
        { "client_id", _microsoftAuthSettings.ClientId },
        { "client_secret", _microsoftAuthSettings.ClientSecret },
        { "redirect_uri", _host + "signin-microsoft" },
        { "code", token },
        { "grant_type", "authorization_code" }
      };

      var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
      var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/common/oauth2/v2.0/token");
      requestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      requestMessage.Content = requestContent;
      var response = await client.SendAsync(requestMessage);
      var payloadStr = await response.Content.ReadAsStringAsync();
      var payload = JObject.Parse(payloadStr);

      if (payload["error"] != null)
      {
        var err = payload["error"];
        _logger.LogWarning("Microsoft token error response: {0}", payloadStr);
        return new MicrosoftUserProfile
        {
          IsSuccessful = false,
          Error = new OAuthError
          {
            Code = payload.Value<string>("error"),
            Message = payload.Value<string>("error_description")
          }
        };
      }

      var graphMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
      graphMessage.Headers.Add("Authorization", "Bearer " + payload["access_token"]);
      var graphResponse = await client.SendAsync(graphMessage);
      var graphPayloadStr = await graphResponse.Content.ReadAsStringAsync();
      var graphPayload = JObject.Parse(graphPayloadStr);

      if (graphPayload["error"] != null)
      {
        var err = graphPayload["error"];
        _logger.LogWarning("Microsoft graph error response: {0}", graphPayloadStr);
        return new MicrosoftUserProfile
        {
          IsSuccessful = false,
          Error = new OAuthError
          {
            Code = err.Value<string>("code"),
            Message = err.Value<string>("message")
          }
        };
      }

      return new MicrosoftUserProfile
      {
        IsSuccessful = true,
        DisplayName = graphPayload.Value<string>("displayName"),
        Mail = graphPayload.Value<string>("mail") ?? graphPayload.Value<string>("userPrincipalName"),
        MobilePhone = graphPayload.Value<string>("mobilePhone")
      };

    }

    catch (Exception ex)
    {
      _logger.LogError("Exception: {0} Details: {1}", ex, ex.StackTrace);
      throw;
    }
  }

}

public interface IOAuthUserProfile
{
  bool IsSuccessful { get; set; }
  OAuthError Error { get; set; }
  string Id { get; set; }
  string Mail { get; set; }
  string JobTitle { get; set; }
  string DisplayName { get; set; }
  string MobilePhone { get; set; }
}

public class MicrosoftUserProfile : IOAuthUserProfile
{
  public bool IsSuccessful { get; set; }
  public OAuthError Error { get; set; }
  public string Id { get; set; }
  public string Mail { get; set; }
  public string JobTitle { get; set; }
  public string DisplayName { get; set; }
  public string MobilePhone { get; set; }
}

public record OAuthError
{
  public string Code { get; set; }
  public string Message { get; set; }
}

