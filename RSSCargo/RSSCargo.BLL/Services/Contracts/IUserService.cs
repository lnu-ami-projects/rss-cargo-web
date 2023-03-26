namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserService
{
    public int LoginUser(string email, string password);
}