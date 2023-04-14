using Blog.App.Content.Services;
using Blog.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Http.Controller;

[Controller, Route("/sitemap.xml")]
public class DynamicSitemap : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly ChannelAccessService ChannelAccessService;
    private readonly AuthorAccessService AuthorAccessService;

    public DynamicSitemap(ChannelAccessService channelAccessService, AuthorAccessService authorAccessService)
    {
        ChannelAccessService = channelAccessService;
        AuthorAccessService = authorAccessService;
    }

    // GET
    public IActionResult Index(string path)
    {
        string sitemap =
            @"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.sitemaps.org/schemas/sitemap/0.9             http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"">";

        void Add(string url, string lastmod)
        {
            sitemap += $"<url><loc>https://{HttpContext.Request.Host}{url}</loc><lastmod>{lastmod}</lastmod></url>";
        }
        
        var channels = ChannelAccessService.GetAllChannels();
        
        Add("/", DateTime.Now.ToString());

        foreach (var channel in channels)
        {
            Add("/" + channel.Id, DateTime.Now.ToString());
            var posts = channel.GetPosts();
            foreach (var post in posts)
            {
                Add($"/{channel.Id}/{post.Id}/", post.Date);
            }
        }

        var authors = AuthorAccessService.GetAllAuthors();
        
        foreach (var author in authors)
        {
            Add("/author/" + author.Id, DateTime.Now.ToString());
        }

        sitemap += "</urlset>";

        return Content(sitemap, "application/xml");
    }
}

