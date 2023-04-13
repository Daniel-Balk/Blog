using Blog.App.Content.Models;
using Blog.App.Content.Models.Configuration;
using Blog.App.Services;
using yml.Net;

namespace Blog.App.Content.Services;

public class PostAccessService
{
    private readonly RootStorageAccessorService RootStorageAccessorService;
    private readonly AuthorAccessService AuthorAccessService;

    private static List<Post>? Posts = null;
    
    public PostAccessService(RootStorageAccessorService rootStorageAccessorService, AuthorAccessService authorAccessService)
    {
        RootStorageAccessorService = rootStorageAccessorService;
        AuthorAccessService = authorAccessService;
    }

    public void Flush()
    {
        Posts = null;
    }

    public Post[] GetAllPosts()
    {
        if (Posts != null)
            return Posts.ToArray();

        Posts = new();

        var dirsInRoot = RootStorageAccessorService.GetDirectories("/");

        foreach (var directory in dirsInRoot)
        {
            var channelConfigLocation = directory.TrimEnd('/').TrimEnd('\\') + "/channel.meta";
            if (RootStorageAccessorService.Exists(channelConfigLocation))
            {
                var dirs = RootStorageAccessorService.GetDirectories(directory);
                var categoryId = directory.Replace("\\", "/").Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
                foreach (var dir in dirs)
                {
                    var postConfigLocation = dir.TrimEnd('/').TrimEnd('\\') + "/article.meta";
                    if (RootStorageAccessorService.Exists(postConfigLocation))
                    {
                        string configuration = RootStorageAccessorService.ReadAllText(postConfigLocation);

                        var meta = YmlConvert.Deserialize<PostMetaModel>(configuration);
                        var titleImageUrl = CompleteUrl("/resources/" + categoryId + "/" + meta.Id + "/" + meta.Icon);

                        var post = new Post(AuthorAccessService, RootStorageAccessorService)
                        {
                            Author = meta.Author,
                            Description = meta.Description,
                            Icon = meta.Icon,
                            Id = meta.Id,
                            ImagePath = titleImageUrl,
                            Title = meta.Title,
                            MarkdownFileLocation = dir.TrimEnd('\\').TrimEnd('/') + "/index.md",
                            CategoryId = categoryId,
                            Date = meta.Date
                        };

                        Posts.Add(post);
                    }
                }
            }
        }

        return Posts.ToArray();
    }

    private string TrimStartString(string t, string u)
    {
        if (!t.StartsWith(u))
            return t;
        return t.Remove(0, u.Length);
    }

    private string CompleteUrl(string url)
    {
        return TrimStartString(Path.Combine(url), "C:\\").Replace("\\", "/");
    }
}