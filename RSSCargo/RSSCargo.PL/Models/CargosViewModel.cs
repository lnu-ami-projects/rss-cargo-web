using RSSCargo.DAL.Models;

namespace RSSCargo.PL.Models;

public class CargosViewModel
{
    public IEnumerable<Cargo> Cargos { get; init; }
    
    public Dictionary<int, List<string>> CargoFeeds { get; init; }
}