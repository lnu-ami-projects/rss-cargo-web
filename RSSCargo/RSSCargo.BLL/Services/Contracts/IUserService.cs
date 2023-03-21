namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserService
{
    Task<List<User>> GetUsers();
}