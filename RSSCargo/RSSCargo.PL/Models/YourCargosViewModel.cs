using RSSCargo.DAL.Models;

namespace RSSCargo.PL.Models;

public class YourCargosViewModel
{
    public IEnumerable<Cargo> SubCargos { get; init; }
    public IEnumerable<Cargo> UnsubCargos { get; init; }

    public Dictionary<int, List<string>> CargoFeeds { get; init; }
}