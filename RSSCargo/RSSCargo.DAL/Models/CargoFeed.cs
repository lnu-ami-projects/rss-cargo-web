namespace RSSCargo.DAL.Models;

public class CargoFeed: IEntityBase
{
    public int Id { get; set; }

    public int CargoId { get; set; }

    public string RssFeed { get; set; } = string.Empty;

    public virtual Cargo Cargo { get; set; } = new();
}
