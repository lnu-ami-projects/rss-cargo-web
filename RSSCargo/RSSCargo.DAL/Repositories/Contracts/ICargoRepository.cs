using RSSCargo.DAL.Models;

namespace RSSCargo.DAL.Repositories.Contracts;

public interface ICargoRepository
{
    public IEnumerable<Cargo> GetAllCargos();
    public IEnumerable<CargoFeed> GetCargoFeeds(int cargoId);
}