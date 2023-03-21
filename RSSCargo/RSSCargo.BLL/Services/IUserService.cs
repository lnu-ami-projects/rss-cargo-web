namespace RSSCargo.BLL.Services;

using DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using Contracts;

public class UserService: IUserService
{
    private readonly IGenericRepository<User> _repository;

    public UserService(IGenericRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<List<User>> GetUsers()
    {
        try
        {
            return await _repository.GetUsers();
        }
        catch
        {
            throw new Exception();
        }
    }
}