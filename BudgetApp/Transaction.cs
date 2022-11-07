using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp;

public class Transaction
{
  [Key, Required]
  public ulong TransactionID { get; set; }
  [Required]
  public int AmountCents { get; set; }
  [Required]
  public string Category { get; set; }
  public string Note { get; set; }
  [Required]
  public DateTime Day { get; set; }
  [Required]
  public Guid UserId { get; set; }

  public Transaction() { }
}