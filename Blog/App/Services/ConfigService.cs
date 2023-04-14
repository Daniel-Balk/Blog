using Blog.App.Models.Configuration;
using yml.Net;

namespace Blog.App.Services;

public class ConfigService
{
    public ConfigService()
    {
        _ = Get();
    }

    private BlogSettings? _config = null;
    
    public BlogSettings Config
    {
        get => _config ?? Get();
        set => _config = value;
    }

    public BlogSettings Get()
    {
        string configPath = GetConfigPath();

        if (File.Exists(configPath))
        {
            var yaml = File.ReadAllText(configPath);
            var config = YmlConvert.Deserialize<BlogSettings>(yaml);

            _config = config;

            return config;
        }
        else
        {
            var config = new BlogSettings();

            _config = config;
            Save();

            return config;
        }
    }

    public void Save()
    {
        var yaml = YmlConvert.Serialize(_config);
        
        File.WriteAllText(GetConfigPath(), yaml);
    }

    public string GetConfigPath()
    {
        return 
#if DEBUG
        "../../blogConfig.yml"
#else
        "config.yml"
#endif
            ;
    }

    public void Flush()
    {
        _config = null;
    }
}