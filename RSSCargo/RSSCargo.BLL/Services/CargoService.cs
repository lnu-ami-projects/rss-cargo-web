using System.Collections;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Rss;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;

namespace RSSCargo.BLL.Services;

public class CargoService : ICargoService
{
    private readonly ICargoRepository _repository;
    private readonly IUserCargoService _userCargoService;

    public CargoService(ICargoRepository repository, IUserCargoService userCargoService)
    {
        _repository = repository;
        _userCargoService = userCargoService;
    }

    public IEnumerable<Cargo> GetAllCargos()
    {
        return _repository.GetAllCargos();
    }

    public IEnumerable<Cargo> GetUnsubscribedCargos(int userId)
    {
        var userCargosIds = _userCargoService
            .GetUserCargos(userId)
            .Select(x => x.CargoId)
            .ToArray();

        var cargos = _repository.GetAllCargos();

        return cargos.Where(c => !userCargosIds.Contains(c.Id));
    }

    public IEnumerable<Cargo> GetSubscribedCargos(int userId)
    {
        var userCargosIds = _userCargoService
            .GetUserCargos(userId)
            .Select(x => x.CargoId)
            .ToArray();

        var cargos = _repository.GetAllCargos();

        return cargos.Where(c => userCargosIds.Contains(c.Id));
    }

    public IEnumerable<CargoFeed> GetCargoFeeds(int cargoId)
    {
        return _repository.GetCargoFeeds(cargoId);
    }

    public Cargo GetCargoById(int cargoId)
    {
        return _repository.GetAllCargos().First(c => c.Id == cargoId);
    }

    public IEnumerable<RssFeed> GetRssCargoFeeds(int cargoId)
    {
        var feeds = GetCargoFeeds(cargoId)
            .Select(cargoCargoFeed => new RssFeed(0, cargoCargoFeed.RssFeed))
            .ToList();

        return feeds;
    }
}