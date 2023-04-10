using RSSCargo.BLL.Services.Rss;
using RSSCargo.DAL.Models;

namespace RSSCargo.PL.Models;

public class CargosViewModel
{
    public IEnumerable<Cargo> Cargos { get; init; }
    
    // public IEnumerable<Tuple<int, string>> CargoFeeds { get; init; }
}