using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp;

public class BudgetCategory
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int CategoryId { get; set; }

  [Required]
  public string Name { get; set; }

  public int BudgetedAmountCents { get; set; }
  public int CurrentAmountCents { get; set; }
  
  public int MonthlyBudgetId { get; set; }
  public virtual MonthlyBudget MonthlyBudget { get; set; }

  public Guid UserId { get; set; }
  public virtual User User { get; set; }


  public List<Transaction> Transactions { get; set; }

  public BudgetCategory() { }
}