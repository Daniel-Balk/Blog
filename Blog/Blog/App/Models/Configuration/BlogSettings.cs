namespace Blog.App.Models.Configuration;

public class BlogSettings
{
    public DatabaseConfiguration Database { get; set; } = new();
    public string DataBasePath { get; set; } = "";
    public string JwtSecret { get; set; } = "";
}