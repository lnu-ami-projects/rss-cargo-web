using Microsoft.AspNetCore.Http;

namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserService
{
    public void UserAuthenticated(HttpContext ctx, int userId, string username);
    public User? GetUserByEmail(string email);
    public User? GetUserById(int id);
    public User? GetUserAuthenticated(HttpContext ctx);
    public void CreateUser(string email, string username, string password);
}