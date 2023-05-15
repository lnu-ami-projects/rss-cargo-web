using RSSCargo.DAL.Models.Contracts;

namespace RSSCargo.DAL.Models;

public sealed class Cargo : IEntityBase
{
    public int Id { get; set; }

    public string Name { get; init; } = string.Empty;

    public IEnumerable<CargoFeed> CargoFeeds { get; init; } = null!;

    public IEnumerable<UserCargo> UserCargos { get; init; } = null!;
}