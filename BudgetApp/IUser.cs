namespace BudgetApp;

public interface IUser
{
  public void Add(User user);
  public User Get(Guid Id);
  public void Update(Guid Id);
  public void Delete(Guid Id);
  void Commit();
}