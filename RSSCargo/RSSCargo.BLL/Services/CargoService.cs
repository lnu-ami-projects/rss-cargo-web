using RSSCargo.BLL.Services.Contracts;
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

    public IEnumerable<Cargo> GetUnsubscribeCargos(int userId)
    {
        var userCargosIds = _userCargoService
            .GetUserCargos(userId)
            .Select(x => x.CargoId)
            .ToArray();

        var cargos = _repository.GetAllCargos();

        return cargos.Where(c => !userCargosIds.Contains(c.Id));
    }

    public IEnumerable<CargoFeed> GetCargoFeeds(int cargoId)
    {
        return _repository.GetCargoFeeds(cargoId);
    }

    public Cargo GetCargoById(int cargoId)
    {
        return _repository.GetAllCargos().First(c => c.Id == cargoId);
    }
}