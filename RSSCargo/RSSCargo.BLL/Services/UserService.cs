using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace RSSCargo.BLL.Services;

using DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using Contracts;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public User? GetUserByEmail(string email)
    {
        return _repository.GetUserByEmail(email);
    }

    public User? GetUserById(int id)
    {
        return _repository.GetUserById(id);
    }

    public User? GetUserAuthenticated(HttpContext ctx)
    {
        var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId == null ? null : _repository.GetUserById(int.Parse(userId));
    }

    public void CreateUser(string email, string username, string password)
    {
        _repository.CreateUser(email, username, password);
    }

    public void UserAuthenticated(HttpContext ctx, int userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(200),
            IssuedUtc = DateTimeOffset.UtcNow,
        };

        ctx.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        ).Wait();
    }
}