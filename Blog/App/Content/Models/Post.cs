using Blog.App.Content.Models.Configuration;
using Blog.App.Content.Services;
using Blog.App.Services;

namespace Blog.App.Content.Models;

public class Post : PostMetaModel
{
    private readonly AuthorAccessService AuthorAccessService;
    private readonly RootStorageAccessorService RootStorageAccessorService;
    
    public string ImagePath { get; set; }
    public string MarkdownFileLocation { get; set; }
    public string CategoryId { get; set; }
    
    public Post(AuthorAccessService authorAccessService, RootStorageAccessorService rootStorageAccessorService)
    {
        AuthorAccessService = authorAccessService;
        RootStorageAccessorService = rootStorageAccessorService;
    }

    public Author GetAuthor()
    {
        return AuthorAccessService.GetAllAuthors().First(x => x.Id == Author);
    }
}