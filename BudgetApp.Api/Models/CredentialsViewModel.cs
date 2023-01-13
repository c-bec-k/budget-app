using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Api.Models;

public record CredentialsViewModel([MaxLength(30), Required] string Email, [MaxLength(72), Required] string Password );

