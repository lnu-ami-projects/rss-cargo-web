namespace RSSCargo.DAL.Models;

public class UserCargo: IEntityBase
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CargoId { get; set; }

    public virtual Cargo Cargo { get; set; } = new();

    public virtual User User { get; set; } = new();
}
