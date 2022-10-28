using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp
{
  public class Transaction
  {
    [Key]
    public int TransactionID { get; set; }
    public int AmountCents { get; set; }
    [Required]
    public string Category { get; set; }
    public string Note { get; set; }
    public DateTime Day { get; set; }
    public Guid UserId { get; set; }

    public Transaction() { }
  }
}