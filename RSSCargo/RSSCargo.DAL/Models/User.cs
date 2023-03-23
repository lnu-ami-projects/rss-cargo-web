namespace RSSCargo.DAL.Models;

public class User: IEntityBase
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;

    public virtual IEnumerable<UserCargo> UserCargos { get; } = new List<UserCargo>();

    public virtual IEnumerable<UserFeed> UserFeeds { get; } = new List<UserFeed>();
}
