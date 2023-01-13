using System;
using Microsoft.IdentityModel.Tokens;

namespace BudgetApp.Api.Auth;

public class JwtIssuerOptions
{
  public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
  public string Issuer { get; set; }
  public string Audience { get; set; }
  public SigningCredentials SigningCredentials { get; set; }
  // 'exp' is a datetime in the future that the JTW stops working
  public DateTime ExpirationTime => IssuedAt.AddMinutes(ValidFor);
  // ValidFor(in min)  will be added to datetime.now() to set the 'exp' field
  public int ValidFor { get; set; }

  public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
}