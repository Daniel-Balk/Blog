using System.Net;
using Zhaobang.FtpServer.Connections;

namespace Blog.App.Services.FTP;

public class FixedLocalDataConnectionFactory : IDataConnectionFactory
{
    private readonly ConfigService ConfigService;
    
    public FixedLocalDataConnectionFactory(ConfigService configService)
    {
        ConfigService = configService;
    }
    
    public IDataConnection GetDataConnection(IPAddress localIP) => (IDataConnection) new LocalDataConnection(IPAddress.Parse(ConfigService.Get().FTP.IP));
}