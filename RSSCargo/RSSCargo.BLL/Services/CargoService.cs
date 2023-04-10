using RSSCargo.BLL.Services.Contracts;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;

namespace RSSCargo.BLL.Services;

public class CargoService: ICargoService
{
    private readonly ICargoRepository _repository;

    public CargoService(ICargoRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<Cargo> GetAllCargos()
    {
        return _repository.GetAllCargos();
    }

    public IEnumerable<Cargo> GetUnsubscribeCargos(IEnumerable<int> userCargos)
    {
        return GetAllCargos().Where(c => !userCargos.Contains(c.Id));
    }
}