namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserService
{
    public User? LoginUser(string email, string password);
}