using RSSCargo.DAL.DataContext;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;

namespace RSSCargo.DAL.Repositories;

public class CargoRepository: ICargoRepository
{
    private readonly RssCargoContext _context;

    public CargoRepository(RssCargoContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Cargo> GetAllCargos()
    {
        return _context.Cargos;
    }
    
    public IEnumerable<CargoFeed> GetCargoFeeds(int cargoId)
    {
        return _context.CargoFeeds.Where(c => c.CargoId == cargoId);
    }
}