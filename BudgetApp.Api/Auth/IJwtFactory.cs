using System;
using System.Security.Claims;

namespace BudgetApp.Api.Auth;

public interface IJwtFactory
{
  // Generates a full JWT
  // userName is the `sub` of the JWT
  // identity is created via `GenerateClaimsIdentity`

  Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);


  // Generate `identity` for JWT that contains ApiAccess role claim
  // userName is the username of the authenticated user
  // id is the user's UUID

  ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
}

