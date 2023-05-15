using RSSCargo.DAL.Models;

namespace RSSCargo.PL.Models;

public class YourCargosViewModel
{
    public IEnumerable<Cargo> SubCargos { get; init; } = new List<Cargo>();
    public IEnumerable<Cargo> UnsubCargos { get; init; } = new List<Cargo>();

    public Dictionary<int, List<string>> CargoFeeds { get; init; } = new();
}