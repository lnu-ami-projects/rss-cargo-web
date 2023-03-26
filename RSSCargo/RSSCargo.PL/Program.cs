using Microsoft.EntityFrameworkCore;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services;
using RSSCargo.DAL.DataContext;
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

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RssCargoContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetValue<string>("CONNECTION_STRING"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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
    app.UseAuthorization();

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