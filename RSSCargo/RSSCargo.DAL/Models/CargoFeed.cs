namespace RSSCargo.DAL.Models;

public class CargoFeed
{
    public int Id { get; set; }

    public int CargoId { get; set; }

    public string RssFeed { get; set; } = null!;

    public virtual Cargo Cargo { get; set; } = null!;
}
