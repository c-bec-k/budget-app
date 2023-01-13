using System;
namespace BudgetApp.Api.Auth;

public record MicrosoftAuthViewModel(string AccessToken, string State, string BaseHref);
