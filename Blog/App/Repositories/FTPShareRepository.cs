using Blog.App.Database;
using Blog.App.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.App.Repositories;

public class FTPShareRepository
{
    private readonly DataContext DataContext;
    
    public FTPShareRepository(DataContext dataContext)
    {
        DataContext = dataContext;
    }

    public DbSet<FTPShare> Get()
    {
        return DataContext.FtpShares;
    }

    public FTPShare Add(FTPShare ftpShare)
    {
        var u = DataContext.FtpShares.Add(ftpShare).Entity;
        DataContext.SaveChanges();
        return u;
    }

    public void Update(FTPShare ftpShare)
    {
        DataContext.FtpShares.Update(ftpShare);
        DataContext.SaveChanges();
    }

    public void Remove(FTPShare ftpShare)
    {
        DataContext.FtpShares.Remove(ftpShare);
        DataContext.SaveChanges();
    }

    public void Dispose()
    {
        DataContext.Dispose();
    }
}