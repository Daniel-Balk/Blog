using Blog.App.Database.Entities;
using Blog.App.Models.FTP;
using Zhaobang.FtpServer;
using Zhaobang.FtpServer.File;

namespace Blog.App.Services.FTP;

public class FileProviderService : IFileProvider
{
    private readonly RootStorageAccessorService RootStorageAccessorService;
    private readonly FTPPermissionManagerService FtpPermissionManagerService;

    private string WorkingDirectory = "/";
    public User? User;
    
    public FileProviderService(RootStorageAccessorService rootStorageAccessorService, FTPPermissionManagerService ftpPermissionManagerService)
    {
        RootStorageAccessorService = rootStorageAccessorService;
        FtpPermissionManagerService = ftpPermissionManagerService;
    }
    
    public async Task CreateDirectoryAsync(string partialPath)
    {
        var path = WorkingDirectory + partialPath;

        Directory.CreateDirectory(RootStorageAccessorService.GetFullPath(path));
        FtpPermissionManagerService.EnsureOwner(User!, path + "/");
    }

    public async Task<Stream> CreateFileForWriteAsync(string partialPath)
    {
        var path = WorkingDirectory + partialPath;

        if (RootStorageAccessorService.Exists(partialPath))
            FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.Write);

        FtpPermissionManagerService.EnsureOwner(User!, path);
        return File.OpenWrite(RootStorageAccessorService.GetFullPath(path));
    }

    public async Task DeleteAsync(string path)
    {        
        FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.Delete);

        RootStorageAccessorService.Delete(path);
    }

    public async Task DeleteDirectoryAsync(string path)
    {
        FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.Delete);

        RootStorageAccessorService.DirDelete(path);
    }

    public async Task<IEnumerable<FileSystemEntry>> GetListingAsync(string partialPath)
    {
        var path = WorkingDirectory + partialPath;
        
        FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.ListDirectories);
        FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.ListFiles);
        var list = new List<FileSystemEntry>();

        var dirs = RootStorageAccessorService.GetDirectories(path);
        var files = RootStorageAccessorService.GetFiles(path, "*");

        foreach (var dir in dirs)
        {
            var name = dir.Split('/', '\\').Last();
            var readOnly = !FtpPermissionManagerService.IsActionPermitted(User!, dir + "/", FTPPermission.Write);

            if (FtpPermissionManagerService.IsActionPermitted(User!, FtpPermissionManagerService.GetFTPPath(dir + "/"),
                    FTPPermission.Read))
                list.Add(new FileSystemEntry()
                {
                    IsDirectory = true,
                    Name = name,
                    IsReadOnly = readOnly,
                    LastWriteTime = Directory.GetLastWriteTime(dir),
                    Length = 0
                });
        }

        foreach (var file in files)
        {
            var name = file.Split('/', '\\').Last();
            var readOnly = !FtpPermissionManagerService.IsActionPermitted(User!, file, FTPPermission.Write);

            if (FtpPermissionManagerService.IsActionPermitted(User!, FtpPermissionManagerService.GetFTPPath(file),
                    FTPPermission.Read))
                list.Add(new FileSystemEntry()
                {
                    IsDirectory = false,
                    Name = name,
                    IsReadOnly = readOnly,
                    LastWriteTime = File.GetLastWriteTime(file),
                    Length = new FileInfo(file).Length
                });
        }
        
        return list;
    }

    public string GetWorkingDirectory()
    {
        return WorkingDirectory;
    }

    public async Task<IEnumerable<string>> GetNameListingAsync(string partialPath)
    {
        var path = WorkingDirectory + partialPath;
        
        FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.Read);
        var list = new List<string>();

        var dirs = RootStorageAccessorService.GetDirectories(path);
        var files = RootStorageAccessorService.GetFiles(path, "*");

        foreach (var dir in dirs)
        {
            var name = dir.Split('/', '\\').Last();
            list.Add(name);
        }

        foreach (var file in files)
        {
            var name = file.Split('/', '\\').Last();
            list.Add(name);
        }
        
        return list;
    }

    public async Task<Stream> OpenFileForReadAsync(string partialPath)
    {
        var path = WorkingDirectory + partialPath;
        FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.Read);

        return File.OpenRead(RootStorageAccessorService.GetFullPath(path));
    }

    public async Task<Stream> OpenFileForWriteAsync(string partialPath)
    {
        var path = WorkingDirectory + partialPath;

        if (RootStorageAccessorService.Exists(partialPath))
            FtpPermissionManagerService.ForcePermission(User!, path, FTPPermission.Write);

        FtpPermissionManagerService.EnsureOwner(User!, path);
        return File.OpenWrite(RootStorageAccessorService.GetFullPath(path));
    }

    public async Task RenameAsync(string fromPath, string toPath)
    {
        var isFile = RootStorageAccessorService.Exists(fromPath);
        FtpPermissionManagerService.ForcePermission(User!, fromPath, FTPPermission.Move);
        FtpPermissionManagerService.EnsureOwner(User!, toPath);

        if (isFile)
            File.Move(RootStorageAccessorService.GetFullPath(fromPath), RootStorageAccessorService.GetFullPath(toPath));
        else
            Directory.Move(RootStorageAccessorService.GetFullPath(fromPath),
                RootStorageAccessorService.GetFullPath(toPath));
        FtpPermissionManagerService.Move(fromPath, toPath);
    }

    public bool SetWorkingDirectory(string path)
    {
        var newPath = path.StartsWith("/") ? path : WorkingDirectory + path + "/";
        
        while (newPath.Contains("//"))
        {
            newPath = newPath.Replace("//", "/");
        }
        
        if (!RootStorageAccessorService.DirExists(newPath))
            return false;
        
        WorkingDirectory = newPath;

        if (!WorkingDirectory.EndsWith("/"))
            WorkingDirectory += "/";
        
        return true;
    }
}