using System;

namespace BudgetApp;

public class MonthlyBudget
{
  public int MonthlyBudgetID { get; set; }
  public DateOnly Month { get; set; }
  public List<BudgetCategory> Spending { get; set; }
  public Guid UserID { get; set; }
  public int TotalBudgetCents { get; set; }

  public MonthlyBudget() { }
}