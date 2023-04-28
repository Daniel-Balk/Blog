using Blog.App.Content.Services;
using Blog.App.Database;
using Blog.App.Repositories;
using Blog.App.Services;
using Blog.App.Services.FTP;
using Blog.App.Services.Sessions;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

var services = builder.Services;

// Http Context Accessor
services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Database
services.AddScoped<DataContext>();

// Repositories
services.AddScoped<UserRepository>();
services.AddScoped<CommentRepository>();
services.AddScoped<FTPShareRepository>();
services.AddScoped<FTPOwnerEntryRepository>();

// Services
services.AddScoped<ConfigService>();
services.AddScoped<RootStorageAccessorService>();
services.AddScoped<CookieService>();
services.AddScoped<IdentityService>();
services.AddScoped<UserService>();
services.AddScoped<AuthorAccessService>();
services.AddScoped<ChannelAccessService>();
services.AddScoped<PostAccessService>();
services.AddScoped<CacheFlushService>();
services.AddScoped<CommentOrderService>();
services.AddScoped<IpTrackingService>();
services.AddScoped<AuthenticatorService>();
services.AddScoped<FileProviderService>();
services.AddScoped<FTPPermissionManagerService>();

services.AddSingleton<FTPServerService>();
services.AddSingleton<FileProviderFactoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapControllers();
app.MapFallbackToPage("/_Host");

_ = app.Services.GetService<FTPServerService>();

app.Run();
