using RSSCargo.DAL.Models;

namespace RSSCargo.BLL.Services.Contracts;

public interface ICargoService
{
    public IEnumerable<Cargo> GetAllCargos();

    public IEnumerable<Cargo> GetUnsubscribeCargos(IEnumerable<int> userCargos);
}