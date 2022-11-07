using System;

namespace BudgetApp.SqlDbServices;

public class SQLUserData : IUser
{

  private readonly BudgetAppDbContext _ctx;

  public SQLUserData(BudgetAppDbContext ctx)
  {
    var _ctx = ctx;
  }

  public void Add(User user)
  {
    _ctx.Users.Add(user);
  }

  public User Get(Guid Id)
  {
    return _ctx.Users.FirstOrDefault<User>(x => x.Id == Id);
  }

  public void Update(Guid Id)
  {
    throw new NotImplementedException();
  }

  public void Delete(Guid Id)
  {
    User user = _ctx.Users.FirstOrDefault<User>(x => x.Id == Id);
    _ctx.Users.Remove(user);
  }

  public void Commit()
  {
    _ctx.SaveChanges();
  }
}

