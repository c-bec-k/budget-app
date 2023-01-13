// View Models are what are returned to the client

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Api.Models;

public class LoginUserViewModel
{
  [Required(), DisplayName("Email"), MaxLength(256), DataType(DataType.EmailAddress)]
  public string Email { get; set; }

  [Required(), DataType(DataType.Password)]
  public string Password { get; set; }

  public LoginUserViewModel(string email, string password)
  {
    Email = email;
    Password = password;
  }
}

