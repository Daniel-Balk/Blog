using Blog.App.Content.Services;

namespace Blog.App.Services;

public class CacheFlushService
{
    private readonly ConfigService ConfigService;
    private readonly AuthorAccessService AuthorAccessService;
    private readonly ChannelAccessService ChannelAccessService;
    private readonly PostAccessService PostAccessService;
    
    public CacheFlushService(ConfigService configService, AuthorAccessService authorAccessService,
        ChannelAccessService channelAccessService, PostAccessService postAccessService)
    {
        ConfigService = configService;
        AuthorAccessService = authorAccessService;
        ChannelAccessService = channelAccessService;
        PostAccessService = postAccessService;
    }

    public void Flush()
    {
        ConfigService.Flush();
        AuthorAccessService.Flush();
        ChannelAccessService.Flush();
        PostAccessService.Flush();
    }
}