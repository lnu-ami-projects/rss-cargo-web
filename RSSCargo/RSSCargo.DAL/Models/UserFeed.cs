namespace RSSCargo.DAL.Models;

public class UserFeed: IEntityBase
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RssFeed { get; set; } = string.Empty;

    public virtual User User { get; set; }
}
