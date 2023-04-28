using Blog.App.Repositories;
using Zhaobang.FtpServer.File;

namespace Blog.App.Services.FTP;

public class FileProviderFactoryService : IFileProviderFactory
{
    private readonly IServiceScopeFactory ServiceScopeFactory;
    
    public FileProviderFactoryService(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
    }
    
    public IFileProvider GetProvider(string username)
    {
        var provider = ServiceScopeFactory.CreateScope().ServiceProvider.GetService<FileProviderService>();
        var userRepository = ServiceScopeFactory.CreateScope().ServiceProvider.GetService<UserRepository>();

        var user = userRepository!.Get().FirstOrDefault(x => x.Username == username);

        provider.User = user;
        
        return provider!;
    }
}