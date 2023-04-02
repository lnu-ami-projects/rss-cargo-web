namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserService
{
    public int SignInUser(string email, string password);
}