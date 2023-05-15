using RSSCargo.DAL.Models.Contracts;

namespace RSSCargo.DAL.Models;

public sealed class CargoFeed : IEntityBase
{
    public int Id { get; set; }

    public int CargoId { get; init; }

    public string RssFeed { get; init; } = string.Empty;

    public Cargo Cargo { get; set; } = null!;
}