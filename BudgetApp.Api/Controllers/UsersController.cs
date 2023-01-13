using System;
using System.Text.Json;
using BudgetApp.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Api.Controllers;

[Route("api/[controller]")]
public class UsersController : Controller
{
  private readonly UserManager<User> _userManager;
  private readonly SignInManager<User> _signInManager;

  public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
  {
    this._userManager = userManager;
    this._signInManager = signInManager;
  }

  [HttpPost]
  public async Task<IActionResult> Register(RegisterUserViewModel model)
  {
    if (!ModelState.IsValid) return BadRequest();

    if (model.Password != model.ConfirmPassword) return BadRequest();

    User newUser = new()
    {
      DisplayName = model.Email,
      Email = model.Email
    };

    var createdResult = await this._userManager.CreateAsync(newUser, model.Password);

    if (!createdResult.Succeeded)
    {
      return StatusCode(StatusCodes.Status500InternalServerError);
    }

    await _signInManager.SignInAsync(newUser, isPersistent: true);
    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Login(LoginUserViewModel model)
  {
    if (!ModelState.IsValid) return BadRequest();

    var loginUser = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

    if (!loginUser.Succeeded) return BadRequest("Invalid email and/or password");

    var user = await _userManager.FindByEmailAsync(model.Email);

    if (user.Email != "") return BadRequest("Invalid email and/or password");

    return Ok(user);
  }

}

[Route("/signin-microsoft")]
public class MicrosoftLoginController : Controller
{
  private readonly IConfiguration config;

  public MicrosoftLoginController(IConfiguration configuration)
  {
    config = configuration;
  }
  [HttpGet("/login")]
  public async Task<IActionResult> Login([FromQuery] string code)
  {
    // https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=8ff4572f-864d-4691-85fa-39b12d1f9869&response_type=code&redirect_uri=http%3A%2F%2Flocalhost%3A18699%2Fsignin-microsoft&scope=openid%20email%20profile
    Console.WriteLine($"Code attained! {code}");

    // Exchange auth code for needed info
    using HttpClient client = new();
    List<KeyValuePair<string, string>> newParams = new()
    {
      KeyValuePair.Create("client_ID", config.GetValue<string>("MicrosoftLogin:ClientId")),
      KeyValuePair.Create("scope", "User.read"),
      KeyValuePair.Create("code", code),
      KeyValuePair.Create("redirect_uri", "http://localhost:18699/signin-microsoft"),
      KeyValuePair.Create("grant_type", "authorization_code"),
      KeyValuePair.Create("client_secret", config.GetValue<string>("MicrosoftLogin:ClientSecret"))
    };

    Uri address = new("https://login.microsoftonline.com/common/oauth2/v2.0/token");

    FormUrlEncodedContent payload = new(newParams);
    payload.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
    Console.WriteLine($"Payload: {await payload.ReadAsStringAsync()}");

    var result = await client.PostAsync(address, payload);
    var user = JsonSerializer.Deserialize<MSUser>(await result.Content.ReadAsStreamAsync());
    Console.WriteLine($"Does this work? {user}");

    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user?.access_token}");
    result = await client.GetAsync("https://graph.microsoft.com/v1.0/users/");



    return Ok(await result.Content.ReadAsStringAsync());
  }
  private record MSUser(string token_type, string scope, int expires_in, int ext_expires_in, string access_token, string id_token);
}
