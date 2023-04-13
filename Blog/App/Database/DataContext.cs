using Blog.App.Database.Entities;
using Blog.App.Services;
using Microsoft.EntityFrameworkCore;

namespace Blog.App.Database;

public class DataContext : DbContext
{
    private readonly ConfigService ConfigService;
    
    public DataContext(ConfigService configService)
    {
        ConfigService = configService;
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = ConfigService.Config.Database;

            var connectionString = $"host={config.Host};" +
                                   $"port={config.Port};" +
                                   $"database={config.Database};" +
                                   $"uid={config.Username};" +
                                   $"pwd={config.Password}";
            
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.Parse("5.7.37-mysql"),
                builder =>
                {
                    builder.EnableRetryOnFailure(5);
                }
            );
        }
    }
}