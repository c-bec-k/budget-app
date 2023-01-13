using System;
namespace BudgetApp.Api.Auth;

public static class Constants
{
  public static class JwtClaimIdentifiers
  {
    // Represents user role
    public const string Rol = "rol";

    // Represents user's unique ID
    public const string Id = "id";
  }

  public static class JwtClaims
  {
    // Claims give user basic API access
    public const string ApiAccess = "api_access";
  }
}

