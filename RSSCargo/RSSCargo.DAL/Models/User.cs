namespace RSSCargo.DAL.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual IEnumerable<UserCargo> UserCargos { get; } = new List<UserCargo>();

    public virtual IEnumerable<UserFeed> UserFeeds { get; } = new List<UserFeed>();
}
