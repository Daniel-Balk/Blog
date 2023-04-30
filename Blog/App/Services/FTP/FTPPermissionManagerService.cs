using System.Security.Permissions;
using Blog.App.Database.Entities;
using Blog.App.Models.FTP;
using Blog.App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.App.Services.FTP;

public class FTPPermissionManagerService
{
    private readonly FTPShareRepository FtpShareRepository;
    private readonly FTPOwnerEntryRepository FtpOwnerEntryRepository;
    private readonly ConfigService ConfigService;
    private readonly RootStorageAccessorService RootStorageAccessorService;
    
    public FTPPermissionManagerService(FTPShareRepository ftpShareRepository, FTPOwnerEntryRepository ftpOwnerEntryRepository, ConfigService configService, RootStorageAccessorService rootStorageAccessorService)
    {
        FtpShareRepository = ftpShareRepository;
        FtpOwnerEntryRepository = ftpOwnerEntryRepository;
        ConfigService = configService;
        RootStorageAccessorService = rootStorageAccessorService;
    }

    public string GetFTPPath(string physicalPath)
    {
        var fullPath = RootStorageAccessorService.GetFullPath(physicalPath);
        var fullRootPath = Path.GetFullPath(ConfigService.Get().DataBasePath);

        var relative = fullPath.Remove(0, fullRootPath.Length);

        var ftpPath = relative.Replace("\\", "/");
        
        return ftpPath;
    }

    public bool IsActionPermitted(User user, string path, FTPPermission permission)
    {
        if (user.IsAdmin | user.IsSuperAdmin)
            return true;

        if (!user.IsAuthor)
            return false;

        if (path == "/" && permission.HasFlag(FTPPermission.Read))
            return true;

        var isOwner = FtpOwnerEntryRepository.Get().Include(x => x.User)
            .FirstOrDefault(x => x.User.Id == user.Id && x.Path == path) != null;

        if (isOwner)
            return true;
        
        var share = FtpShareRepository.Get().Include(x => x.User)
            .FirstOrDefault(x => x.User.Id == user.Id && x.Path == path);
        var shareExists = share != null;

        if (!shareExists)
            return false;
        return share!.Permission.HasFlag(permission);
    }

    public void ForcePermission(User user, string path, FTPPermission permission)
    {
        if (IsActionPermitted(user, path, permission))
            return;
        throw new Exception($"The required permission {permission} for '{path}' is missing!");
    }

    public void Move(string from, string to)
    {
        var owner = FtpOwnerEntryRepository.Get().Include(x => x.User).Where(x => x.Path == from);
        var share = FtpShareRepository.Get().Include(x => x.User).Where(x => x.Path == from);

        foreach (var o in owner)
        {
            o.Path = to;
            FtpOwnerEntryRepository.Update(o);
        }

        foreach (var s in share)
        {
            s.Path = to;
            FtpShareRepository.Update(s);
        }
    }

    public void EnsureOwner(User user, string path)
    {
        var exists = FtpOwnerEntryRepository.Get().FirstOrDefault(x => x.Path == path) != null;

        if (!exists)
        {
            try
            {
                Console.WriteLine("Adding Permission for " + user.Id);
                var v = FtpOwnerEntryRepository.Add(new FTPEntryOwner()
                {
                    Path = path,
                    UserId = user.Id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}