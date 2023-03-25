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

    public User? LoginUser(string email, string password)
    {
        var user = _repository.GetUserByEmail(email);
        return user;
    }
}