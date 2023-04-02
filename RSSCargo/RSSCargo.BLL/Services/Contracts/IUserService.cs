using Microsoft.AspNetCore.Http;

namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserService
{
    public void UserAuthenticated(HttpContext ctx, int userId);
    public User? GetUserByEmail(string email);
    public User? GetUserAuthenticated(HttpContext ctx);
}