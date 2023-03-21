namespace RSSCargo.DAL.Models;

public class Cargo
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CargoFeed> CargoFeeds { get; } = new List<CargoFeed>();

    public virtual ICollection<UserCargo> UserCargos { get; } = new List<UserCargo>();
}
