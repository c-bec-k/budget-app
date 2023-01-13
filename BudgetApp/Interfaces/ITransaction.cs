using System;
namespace BudgetApp.Interfaces;

public interface ITransaction
{
  Transaction Get(int Id);
  void Add(Transaction txn);
  void Update(Transaction txn);
  void Delete(int Id);
  void Commit();
}

