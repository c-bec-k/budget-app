using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BudgetApp;

public class User : IdentityUser<Guid>
{

  [Required]
  [MaxLength(50)]
  [DataType("nvarchar(50)")]
  public string DisplayName { get; set; }

  public List<MonthlyBudget> Budgets { get; set; }

  public User() { }
}

