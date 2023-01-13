using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp;

public class Transaction
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int TransactionId { get; set; }
  public int AmountCents { get; set; }

  public int CategoryId { get; set; }
  public virtual BudgetCategory Category { get; set; }
  public string Note { get; set; }
  public DateTime Day { get; set; }

  public Guid UserId { get; set; }
  public virtual User User { get; set; }

  public Transaction() { }
}