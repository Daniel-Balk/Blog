namespace Blog.App.Services;

public class RootStorageAccessorService
{
    private readonly ConfigService ConfigService;
    public string ContentRoot;
    
    public RootStorageAccessorService(ConfigService configService)
    {
        ConfigService = configService;
        ContentRoot = Path.GetFullPath(configService.Config.DataBasePath);
    }

    public string GetFullPath(string path)
    {
        string combined = "";
        if (path.StartsWith(ContentRoot.TrimEnd('/').TrimEnd('\\')))
            combined = path;
        else
            combined = ContentRoot.TrimEnd('/').TrimEnd('\\') + "/" + path.TrimStart('/').TrimStart('\\');
        
        var cleaned = Path.GetFullPath(combined);

        if (cleaned.StartsWith(ContentRoot))
            return cleaned;
        return ContentRoot;
    }

    public string[] GetDirectories(string directory)
    {
        return Directory.GetDirectories(GetFullPath(directory));
    }

    public string[] GetFiles(string directory, string filter)
    {
        return Directory.GetFiles(GetFullPath(directory), filter);
    }

    public string ReadAllText(string fileName)
    {
        return File.ReadAllText(GetFullPath(fileName));
    }

    public bool Exists(string fileName)
    {
        var path = GetFullPath(fileName);
        var exits = File.Exists(path);
        return exits;
    }
}