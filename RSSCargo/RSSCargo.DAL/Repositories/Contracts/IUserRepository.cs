namespace RSSCargo.DAL.Repositories.Contracts;

using Models;

public interface IUserRepository
{
    public User? GetUserByEmail(string email);
}