using Microsoft.AspNetCore.Identity;
using RSSCargo.DAL.Models.Contracts;

namespace RSSCargo.DAL.Models;

public sealed class User : IdentityUser<int>, IEntityBase
{
    public IEnumerable<UserCargo> UserCargos { get; } = new List<UserCargo>();

    public IEnumerable<UserFeed> UserFeeds { get; } = new List<UserFeed>();
}