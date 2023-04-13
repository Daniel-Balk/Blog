using Blog.App.Content.Models;
using Blog.App.Content.Models.Configuration;
using Blog.App.Services;
using yml.Net;

namespace Blog.App.Content.Services;

public class AuthorAccessService
{
    private readonly RootStorageAccessorService RootStorageAccessorService;

    private static List<Author>? Authors = null;
    
    public AuthorAccessService(RootStorageAccessorService rootStorageAccessorService)
    {
        RootStorageAccessorService = rootStorageAccessorService;
    }

    public void Flush()
    {
        Authors = null;
    }

    public Author[] GetAllAuthors()
    {
        if (Authors != null)
            return Authors.ToArray();

        Authors = new();

        var metaFilesInRoot = RootStorageAccessorService.GetFiles("/", "*.meta");

        foreach (var file in metaFilesInRoot)
        {
            string configuration = RootStorageAccessorService.ReadAllText(file);

            var meta = YmlConvert.Deserialize<AuthorMetaModel>(configuration);
            var titleImageUrl = "/resources/" + meta.Id + ".png";

            var author = new Author()
            {
                Description = meta.Description,
                Id = meta.Id,
                ImagePath = titleImageUrl,
                Name = meta.Name
            };

            Authors.Add(author);
        }

        return Authors.ToArray();
    }
}