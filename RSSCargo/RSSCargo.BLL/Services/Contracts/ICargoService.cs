using RSSCargo.DAL.Models;

namespace RSSCargo.BLL.Services.Contracts;

public interface ICargoService
{
    public IEnumerable<Cargo> GetAllCargos();

    public IEnumerable<Cargo> GetUnsubscribeCargos(int userId);
    public IEnumerable<CargoFeed> GetCargoFeeds(int cargoId);
    public Cargo GetCargoById(int cargoId);
}