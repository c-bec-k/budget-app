using System;
namespace BudgetApp.Api.Auth.Models;

public record UserSummaryViewModel(Guid Id, string Name, string Email, string Jwt, string[] Roles);