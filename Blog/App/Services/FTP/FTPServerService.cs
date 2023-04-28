using System.Net;
using Zhaobang.FtpServer;
using Zhaobang.FtpServer.Connections;

namespace Blog.App.Services.FTP;

public class FTPServerService
{
    private readonly IServiceScopeFactory ServiceScopeFactory;
    
    public FTPServerService(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;

        Task.Run(FtpServerMain);
    }

    public async Task FtpServerMain()
    {
        var scope = ServiceScopeFactory.CreateScope();

        var config = scope.ServiceProvider.GetService<ConfigService>();
        
        var endPoint = new IPEndPoint(IPAddress.Any, config!.Get().FTP.Port);

        var fileProviderFactory = scope.ServiceProvider.GetService<FileProviderFactoryService>();
        var dataConnectionFactory = scope.ServiceProvider.GetService<FixedLocalDataConnectionFactory>();
        var authenticator = scope.ServiceProvider.GetService<AuthenticatorService>();

        var server = new FtpServer(endPoint, fileProviderFactory, dataConnectionFactory, authenticator);

        var cancelSource = new CancellationTokenSource();
        await server.RunAsync(cancelSource.Token);
    }
}