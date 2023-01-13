using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp;

public class MonthlyBudget
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int MonthlyBudgetId { get; set; }
  public DateOnly Month { get; set; }
  public List<BudgetCategory> Spending { get; set; }

  public Guid UserId { get; set; }
  public virtual User User { get; set; }

  public int TotalBudgetCents { get; set; }

  public MonthlyBudget() { }
}