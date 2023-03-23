using Serilog;

using RSSCargo.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
// const string envPath = ".env";
// if (File.Exists(envPath))
// {
//     
//     foreach (var line in File.ReadAllLines(envPath))
//     {
//         var parts = line.Split(
//             '=', 2,
//             StringSplitOptions.RemoveEmptyEntries);
//         
//         if (parts.Length != 2)
//             continue; 
//         Environment.SetEnvironmentVariable(parts[0], parts[1]);
//     }
// }

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("App is starting");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<RsscargoContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("connectionString"));
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

