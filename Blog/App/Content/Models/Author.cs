using Blog.App.Content.Models.Configuration;

namespace Blog.App.Content.Models;

public class Author : AuthorMetaModel
{
    public string ImagePath { get; set; } = "";
}