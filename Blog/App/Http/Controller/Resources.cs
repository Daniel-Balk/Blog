using Blog.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Http.Controller;

[Controller, Route("/resources/{*path}")]
public class Resources : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly RootStorageAccessorService RootStorageAccessorService;
    private readonly ConfigService ConfigService;
    
    public Resources(RootStorageAccessorService rootStorageAccessorService, ConfigService configService)
    {
        RootStorageAccessorService = rootStorageAccessorService;
        ConfigService = configService;
    }
    
    // GET
    public IActionResult Index(string path)
    {
        if (RootStorageAccessorService.Exists(path))
        {
            var extension = Path.GetExtension(path);

            if (ConfigService.Config.Resources.AllowedFileExtensions.Contains(extension))
            {
                var index = ConfigService.Config.Resources.AllowedFileExtensions
                    .FindIndex(x => x == extension);
                var mimeType = ConfigService.Config.Resources.MappedMimeTypes[index];

                return PhysicalFile(RootStorageAccessorService.GetFullPath(path), mimeType);
            }
        }
        
        return NotFound();
    }
}