namespace Blog.App.Models.Configuration;

public class ResourcesConfiguration
{
    public List<string> AllowedFileExtensions { get; set; } = new();
    public List<string> MappedMimeTypes { get; set; } = new();
}