namespace RSSCargo.DAL.Repositories.Contracts;

using Models;

public interface IUserRepository
{
    public User? GetUserByEmail(string email);
    public User? GetUserById(int id);
    public void CreateUser(string email, string username, string password);
    public IEnumerable<UserFeed> GetUserFeeds(int userId);
    public void AddUserFeed(int userId, string rssFeed);
    public void RemoveUserFeed(int userId, string rssFeed);
    public IEnumerable<UserCargo> GetUserCargos(int userId);
    public void SubscribeUserCargo(int userId, int cargoId);
    public void UnsubscribeUserCargo(int userId, int cargoId);
}