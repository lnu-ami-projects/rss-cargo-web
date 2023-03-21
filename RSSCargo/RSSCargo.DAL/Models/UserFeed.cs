namespace RSSCargo.DAL.Models;

public class UserFeed
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RssFeed { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
