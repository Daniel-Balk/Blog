using Blog.App.Content.Models.Configuration;
using Blog.App.Content.Services;
using Blog.App.Services;
using Microsoft.AspNetCore.Components;

namespace Blog.App.Content.Models;

public class Post : PostMetaModel
{
    private readonly AuthorAccessService AuthorAccessService;
    private readonly RootStorageAccessorService RootStorageAccessorService;
    private readonly ConfigService ConfigService;

    private string _hypertext = "";
    private double readingTime = -1;
    
    public string ImagePath { get; set; }
    public string MarkdownFileLocation { get; set; }
    public string CategoryId { get; set; }
    
    public Post(AuthorAccessService authorAccessService, RootStorageAccessorService rootStorageAccessorService, ConfigService configService)
    {
        AuthorAccessService = authorAccessService;
        RootStorageAccessorService = rootStorageAccessorService;
        ConfigService = configService;
    }

    public Author GetAuthor()
    {
        return AuthorAccessService.GetAllAuthors().First(x => x.Id == Author);
    }

    public MarkupString GetHypertext()
    {
        if (!string.IsNullOrWhiteSpace(_hypertext))
            return (MarkupString)_hypertext;

        _hypertext = new MarkdownReworker().Patch(RootStorageAccessorService.ReadAllText(MarkdownFileLocation));
        
        return (MarkupString)_hypertext;
    }

    public double GetReadingTime()
    {
        if (readingTime != -1d)
            return readingTime;

        var words = new MarkdownReworker().GetWordCount(RootStorageAccessorService.ReadAllText(MarkdownFileLocation));
        readingTime =
            words / (double)ConfigService.Get().ReadingWPM;
        
        readingTime = Math.Ceiling(readingTime);

        return readingTime;
    }
}