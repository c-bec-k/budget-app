using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Api.Models;

public class RegisterUserViewModel
{
  [Required(), DisplayName("Email"), MaxLength(256), DataType(DataType.EmailAddress)]
  public string Email { get; set; }

  [Required(), DataType(DataType.Password)]
  public string Password { get; set; }

  [Required(), DataType(DataType.Password), Compare(nameof(Password))]
  public string ConfirmPassword { get; set; }

  public RegisterUserViewModel(string email, string password, string confirmPassword)
  {
    Email = email;
    Password = password;
    ConfirmPassword = confirmPassword;
  }
}

