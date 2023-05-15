namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserCargoService
{
    public IEnumerable<UserCargo> GetUserCargos(int userId);
    public void SubscribeUserCargo(int userId, int cargoId);
    public void UnsubscribeUserCargo(int userId, int cargoId);
}
