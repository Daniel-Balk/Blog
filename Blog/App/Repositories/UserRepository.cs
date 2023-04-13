using Blog.App.Database;
using Blog.App.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.App.Repositories;

public class UserRepository : IDisposable
{
    private readonly DataContext DataContext;
    
    public UserRepository(DataContext dataContext)
    {
        DataContext = dataContext;
    }

    public DbSet<User> Get()
    {
        return DataContext.Users;
    }

    public User Add(User user)
    {
        var u = DataContext.Users.Add(user).Entity;
        DataContext.SaveChanges();
        return u;
    }

    public void Update(User user)
    {
        DataContext.Users.Update(user);
        DataContext.SaveChanges();
    }

    public void Remove(User user)
    {
        DataContext.Users.Remove(user);
        DataContext.SaveChanges();
    }

    public void Dispose()
    {
        DataContext.Dispose();
    }
}