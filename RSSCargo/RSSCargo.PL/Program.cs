using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Email;
using RSSCargo.BLL.Services;
using RSSCargo.DAL.DataContext;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using RSSCargo.DAL.Repositories;
using Serilog;
using RSSCargo.PL.CustomTokenProviders;

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
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(200);
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/";
            options.LoginPath = "/Account/SignIn";
            options.LogoutPath = "/Account/SignOut";
        }
    );

builder.Services.AddDbContext<RssCargoContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetValue<string>("CONNECTION_STRING"));
    }
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserFeedService, UserFeedService>();
builder.Services.AddScoped<IUserCargoService, UserCargoService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<IRssFeedService, RssFeedService>();

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;
            options.Password.RequiredUniqueChars = 0;

            //options.SignIn.RequireConfirmedEmail = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(0);
            options.Lockout.MaxFailedAccessAttempts = 50000;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;

            options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
        }
    ).AddEntityFrameworkStores<RssCargoContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromDays(3));

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();

try
{
    Log.Information("App is starting");
    var app = builder.Build();

    app.UseExceptionHandler("/Home/Error");

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
    });

    app.UseStaticFiles();
    app.UseSerilogRequestLogging();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        Log.Information("Stopped: " + ex.Message);
        return;
    }

    Log.Fatal(ex, "App failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}