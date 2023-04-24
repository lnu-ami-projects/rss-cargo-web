using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace RSSCargo.BLL.Services;

using DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using Contracts;

public class UserCargoService : IUserCargoService
{
    private readonly IUserRepository _repository;

    public UserCargoService(IUserRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<UserCargo> GetUserCargos(int userId)
    {
        return _repository.GetUserCargos(userId);
    }

    public void SubscribeUserCargo(int userId, int cargoId)
    {
        _repository.SubscribeUserCargo(userId, cargoId);
    }

    public void UnsubscribeUserCargo(int userId, int cargoId)
    {
        var found = _repository.GetUserCargos(userId).First(x => x.CargoId == cargoId);
        _repository.UnsubscribeUserCargo(found);
    }
}