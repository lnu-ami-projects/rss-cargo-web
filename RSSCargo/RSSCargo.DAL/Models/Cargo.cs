namespace RSSCargo.DAL.Models;

public class Cargo: IEntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual IEnumerable<CargoFeed> CargoFeeds { get; init;  }

    public virtual IEnumerable<UserCargo> UserCargos { get; init; }
}
