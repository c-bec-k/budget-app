using System;
namespace BudgetApp.Interfaces;

public interface IMonthlyBudget
{
  MonthlyBudget Get(int Id);
  void Add(MonthlyBudget budget);
  void Update(MonthlyBudget budget);
  void Delete(int Id);
  void Commit();
}

