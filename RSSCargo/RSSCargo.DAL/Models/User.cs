using Microsoft.AspNetCore.Identity;

namespace RSSCargo.DAL.Models;

public class User : IdentityUser<int>, IEntityBase
{
    public virtual IEnumerable<UserCargo> UserCargos { get; } = new List<UserCargo>();

    public virtual IEnumerable<UserFeed> UserFeeds { get; } = new List<UserFeed>();
}