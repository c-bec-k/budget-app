using System;
using BudgetApp.Api.Auth;
using BudgetApp.Api.Auth.Models;
using BudgetApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BudgetApp.Api.ApiControllers;

[Route("api/auth")]
public class AuthController : Controller
{
  private readonly UserManager<User> _userManager;
  private readonly IJwtFactory _jwtFactory;
  private readonly MSAuthSettings _microsoftAuthSettings;
  private readonly IConfiguration _configuration;
  private readonly ILogger<AuthController> _logger;

  public AuthController(UserManager<User> userManager, IJwtFactory jwtFactory,
  IOptions<MSAuthSettings> microsoftAuthSettings, IConfiguration configuration, ILogger<AuthController> logger)
  {
    this._configuration = configuration;
    this._microsoftAuthSettings = microsoftAuthSettings.Value;
    this._userManager = userManager;
    this._jwtFactory = jwtFactory;
    this._logger = logger;
  }

  [HttpGet("external/microsoft")]
  public IActionResult GetMicrosoft()
  {
    return Ok(new
    {
      client_id = _microsoftAuthSettings.ClientId,
      scope = "https://graph.microsoft.com/user.read",
      state = ""
    });
  }


  [HttpPost("external/microsoft")]
  public async Task<IActionResult> PostMicrosoft([FromBody] MicrosoftAuthViewModel model)
  {
    var verifier = new MicrosoftAuthVerifier<AuthController>(_microsoftAuthSettings, _configuration["HttpHost"] + (model.BaseHref ?? "/"), _logger);
    var profile = await verifier.AcquireUser(model.AccessToken);

    if (profile.IsSuccessful == false)
    {
      return StatusCode(StatusCodes.Status400BadRequest, profile.Error.Message);
    }

    if (String.IsNullOrWhiteSpace(profile.Mail))
    {
      return StatusCode(StatusCodes.Status403Forbidden, "Email address is required");
    }

    var UserLoginInfo = new UserLoginInfo("Microsoft", profile.Id, profile.DisplayName);

    var user = await _userManager.FindByEmailAsync(profile.Mail);
    if (user == null)
    {

      user = new User
      {
        DisplayName = profile.DisplayName,
        Email = profile.Mail,
        PhoneNumber = profile.MobilePhone,
        UserName = profile.Mail
      };

      await _userManager.CreateAsync(user);
    }

    var userModel = await GetUserData(user);
    return Ok(userModel);
  }


  [Authorize(Policy = "ApiUser")]
  [HttpPost("verify")] // POST api/auth/verify
  public async Task<IActionResult> Verify()
  {
    if (User.Identity.IsAuthenticated)
    {
      var user = await _userManager.FindByEmailAsync(User.Identity.Name);
      if (user == null)
        return Forbid();
      var userModel = await GetUserData(user);
      return new ObjectResult(userModel);
    }

    return Forbid();
  }


  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return UnprocessableEntity("1 Email or password malformed");
    }

    var user = new User
    {
      UserName = model.Email,
      Email = model.Email
    };

    var createResult = await this._userManager.CreateAsync(user, model.Password);

    if (!createResult.Succeeded)
    {
      // var errorMessage = String.Join('|', createResult.Errors.Select(x => x.Description));
      return UnprocessableEntity($"2 Email or password malformed.");
    }

    user = await Authenticate(model.Email, model.Password);
    if (user == null)
    {
      return UnprocessableEntity("3 Email or password malformed");
    }


    var userModel = await GetUserData(user);

    return Ok(userModel);

  }

  [HttpPost("login")]
  public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
  {
    if (!ModelState.IsValid)
    {
      return UnprocessableEntity(ModelState);
    }

    var user = await Authenticate(credentials.Email, credentials.Password);
    if (user == null)
    {
      return UnprocessableEntity("Invalid username or password");
    }


    var userModel = await GetUserData(user);


    return Ok(userModel);
  }

  private async Task<User?> Authenticate(string emailAddress, string password)
  {
    if (emailAddress == "" || password == "")
    {
      return null;
    }

    var currentUser = await _userManager.FindByEmailAsync(emailAddress);
    if (currentUser == null)
    {
      // random number of ms to deter timing attacks
      var rand = new Random(DateTime.Now.Second).Next(2, 38);
      await Task.Delay(rand);
      return null;
    }

    var isUser = await _userManager.CheckPasswordAsync(currentUser, password);
    if (isUser)
    {
      return currentUser;
    }

    return null;
  }


  private async Task<UserSummaryViewModel?> GetUserData(User user)
  {
    if (user == null) return null;

    var roles = await _userManager.GetRolesAsync(user);
    if (roles.Count == 0)
    {
      roles.Add("prospect");
    }

    var jwt = await _jwtFactory.GenerateEncodedToken(user.UserName, _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id.ToString()));
    var userModel = new UserSummaryViewModel(user.Id, user.UserName, user.Email, jwt, roles.ToArray());

    return userModel;
  }

}

