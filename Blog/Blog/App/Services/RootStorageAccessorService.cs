namespace Blog.App.Services;

public class RootStorageAccessorService
{
    private readonly ConfigService ConfigService;
    
    public RootStorageAccessorService(ConfigService configService)
    {
        ConfigService = configService;
    }
}