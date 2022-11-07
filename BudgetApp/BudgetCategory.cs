using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp;

public class BudgetCategory
{
  public int CategoryID { get; set; }
  [Required]
  public string Name { get; set; }
  public int BudgetedAmountCents { get; set; }
  public ulong MonthlyBudgetID { get; set; }
  public Guid UserId { get; set; }

  // Current amnt will be grabbed from DB by adding all
  // txns from a given month. Is this field needed?
  public int CurrentAmountCents { get; set; }

  public BudgetCategory() { }
}