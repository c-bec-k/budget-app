using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BudgetApp.SqlDbServices;

public class BudgetAppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
  public BudgetAppDbContext(DbContextOptions<BudgetAppDbContext> options) : base(options) { }

  public DbSet<BudgetCategory> BudgetCategories { get; set; }
  public DbSet<MonthlyBudget> monthlyBudgets { get; set; }
  public DbSet<Transaction> Transactions { get; set; }
}

