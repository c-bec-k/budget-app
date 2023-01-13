using System;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace BudgetApp.Api.Auth;

public class JwtFactory : IJwtFactory
{
  private readonly JwtIssuerOptions _jwtOptions;

  public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
  {
    _jwtOptions = jwtOptions.Value;
    ThrowIfInvalidOptions(_jwtOptions);
  }

  public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
  {
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, userName),
        new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat,
          toUnixEpochDate(_jwtOptions.IssuedAt).ToString(),
          ClaimValueTypes.Integer64),
       identity.FindFirst(Constants.JwtClaimIdentifiers.Rol),
       identity.FindFirst(Constants.JwtClaimIdentifiers.Id)
      };

    var jwt = new JwtSecurityToken(
      issuer: _jwtOptions.Issuer,
      audience: _jwtOptions.Audience,
      claims: claims,
      notBefore: DateTime.UtcNow,
      expires: _jwtOptions.ExpirationTime,
      signingCredentials: _jwtOptions.SigningCredentials);

    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    return encodedJwt;


  }

  public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
  {
    return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
    {
        new Claim(Constants.JwtClaimIdentifiers.Id, id),
        new Claim(Constants.JwtClaimIdentifiers.Rol, Constants.JwtClaims.ApiAccess)
      });
  }

  private static long toUnixEpochDate(DateTime date) =>
    (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

  private static void ThrowIfInvalidOptions(JwtIssuerOptions jwtOptions)
  {
    if (jwtOptions == null || jwtOptions.ValidFor <= 0 || jwtOptions.JtiGenerator == null || jwtOptions.SigningCredentials == null)
    {
      throw new ArgumentException("Invalid JWT options");
    }
  }
}