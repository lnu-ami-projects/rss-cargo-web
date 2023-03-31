using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services;
using RSSCargo.DAL.DataContext;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using RSSCargo.DAL.Repositories;
using Serilog;

// Load .env file
const string envPath = ".env";
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        var parts = line.Split(
            '=', 2,
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            continue;
        Environment.SetEnvironmentVariable(parts[0], parts[1]);
    }
}

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RssCargoContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetValue<string>("CONNECTION_STRING"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<RssCargoContext>()
    .AddDefaultTokenProviders();

const string cookieScheme = "RSSCargoScheme";
builder.Services.AddAuthentication(cookieScheme).AddCookie(cookieScheme, options =>
{
    options.AccessDeniedPath = "/account/denied";
    options.LoginPath = "/account/login";
    
});

try
{
    Log.Information("App is starting");
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseStaticFiles();
    app.UseSerilogRequestLogging();
    app.UseRouting();
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.UseSession();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "App failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}