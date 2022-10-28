using System;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.SqlDbServices
{
  public class BudgetAppDbContext : DbContext
  {
    public BudgetAppDbContext(DbContextOptions<BudgetAppDbContext> options) : base(options) { }

    public DbSet<BudgetCategory> BudgetCategories { get; set; }
    public DbSet<MonthlyBudget> monthlyBudgets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
  }
}

