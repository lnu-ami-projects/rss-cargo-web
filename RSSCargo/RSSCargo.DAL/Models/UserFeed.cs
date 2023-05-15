using RSSCargo.DAL.Models.Contracts;

namespace RSSCargo.DAL.Models;

public sealed class UserFeed : IEntityBase
{
    public int Id { get; set; }

    public int UserId { get; init; }

    public string RssFeed { get; init; } = string.Empty;

    public User User { get; set; } = null!;
}