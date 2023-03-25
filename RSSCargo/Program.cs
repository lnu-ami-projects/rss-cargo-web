using Microsoft.EntityFrameworkCore;
using RSSCargo.DAL.DataContext;
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

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RssCargoContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetValue<string>("CONNECTION_STRING"));
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
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Test db connection
    var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<RssCargoContext>();
    foreach (var user in db.Users)
    {
        Console.WriteLine(user.Email);
    }

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