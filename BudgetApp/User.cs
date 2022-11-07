using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BudgetApp;

public class User
{
  [Required()]
  public Guid Id { get; set; }

  [Required()]
  [MaxLength(50)]
  [DataType("nvarchar(50)")]
  public string DisplayName { get; set; }

  [Required()]
  [MaxLength(50)]
  [DataType("nvarchar(50)")]
  public string Email { get; set; }

  public List<MonthlyBudget> Budgets { get; set; }

  public User() { }
}

