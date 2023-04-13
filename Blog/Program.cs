using Blog.App.Content.Services;
using Blog.App.Database;
using Blog.App.Repositories;
using Blog.App.Services;
using Blog.App.Services.Sessions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

var services = builder.Services;

// Database
services.AddScoped<DataContext>();

// Repositories
services.AddScoped<UserRepository>();

// Services
services.AddScoped<ConfigService>();
services.AddScoped<RootStorageAccessorService>();
services.AddScoped<CookieService>();
services.AddScoped<IdentityService>();
services.AddScoped<UserService>();
services.AddScoped<AuthorAccessService>();
services.AddScoped<ChannelAccessService>();
services.AddScoped<PostAccessService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
