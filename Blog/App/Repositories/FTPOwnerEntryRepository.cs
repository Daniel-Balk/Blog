using Blog.App.Database;
using Blog.App.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.App.Repositories;

public class FTPOwnerEntryRepository
{
    private readonly DataContext DataContext;
    
    public FTPOwnerEntryRepository(DataContext dataContext)
    {
        DataContext = dataContext;
    }

    public DbSet<FTPEntryOwner> Get()
    {
        return DataContext.FTPFileOwners;
    }

    public FTPEntryOwner Add(FTPEntryOwner ftpEntryOwner)
    {
        var u = DataContext.FTPFileOwners.Add(ftpEntryOwner).Entity;
        DataContext.SaveChanges();
        return u;
    }

    public void Update(FTPEntryOwner ftpEntryOwner)
    {
        DataContext.FTPFileOwners.Update(ftpEntryOwner);
        DataContext.SaveChanges();
    }

    public void Remove(FTPEntryOwner ftpEntryOwner)
    {
        DataContext.FTPFileOwners.Remove(ftpEntryOwner);
        DataContext.SaveChanges();
    }

    public void Dispose()
    {
        DataContext.Dispose();
    }
}