using Blog.App.Content.Models;
using Blog.App.Content.Models.Configuration;
using Blog.App.Services;
using yml.Net;

namespace Blog.App.Content.Services;

public class ChannelAccessService
{
    private readonly RootStorageAccessorService RootStorageAccessorService;
    private readonly PostAccessService PostAccessService;

    private List<Channel>? Channels = null;
    
    public ChannelAccessService(RootStorageAccessorService rootStorageAccessorService, PostAccessService postAccessService)
    {
        RootStorageAccessorService = rootStorageAccessorService;
        PostAccessService = postAccessService;
    }

    public void Flush()
    {
        Channels = null;
    }

    public Channel[] GetAllChannels()
    {
        if (Channels != null)
            return Channels.ToArray();

        Channels = new();

        var dirsInRoot = RootStorageAccessorService.GetDirectories("/");

        foreach (var dir in dirsInRoot)
        {
            var channelConfigLocation = dir.TrimEnd('/').TrimEnd('\\') + "/channel.meta";
            if (RootStorageAccessorService.Exists(channelConfigLocation))
            {
                string configuration = RootStorageAccessorService.ReadAllText(channelConfigLocation);

                var meta = YmlConvert.Deserialize<ChannelMetaModel>(configuration);
                var titleImageUrl = "/resources/" + meta.Id + "/icon.png";

                var channel = new Channel(PostAccessService)
                {
                    Description = meta.Description,
                    Id = meta.Id,
                    ImagePath = titleImageUrl,
                    Name = meta.Name
                };
                
                Channels.Add(channel);
            }
        }

        return Channels.ToArray();
    }
}