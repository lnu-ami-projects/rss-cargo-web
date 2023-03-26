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

    public int LoginUser(string email, string password)
    {
        var user = _repository.GetUserByEmail(email);
        if (password == "")
        {
            throw new ApplicationException("password empty");
        }

        return user!.Id;
    }
}