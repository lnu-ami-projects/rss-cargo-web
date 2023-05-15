using RSSCargo.DAL.Models.Contracts;

namespace RSSCargo.DAL.Models;

public sealed class UserCargo : IEntityBase
{
    public int Id { get; set; }

    public int UserId { get; init; }

    public int CargoId { get; init; }

    public Cargo Cargo { get; set; } = null!;

    public User User { get; set; } = null!;
}