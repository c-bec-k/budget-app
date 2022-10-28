using System;
using Microsoft.AspNetCore.Identity;

namespace BudgetApp
{
  public class User : IdentityUser<Guid>
  {
    public string DisplayName { get; set; }
    public List<MonthlyBudget> Budgets { get; set; }

    public User() { }
  }
}

