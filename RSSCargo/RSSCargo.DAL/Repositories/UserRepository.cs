using RSSCargo.DAL.DataContext;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;

namespace RSSCargo.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RssCargoContext _context;

    public UserRepository(RssCargoContext context)
    {
        _context = context;
    }

    public User? GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public void CreateUser(string email, string username, string password)
    {
        var user = new User
        {
            Email = email,
            UserName = username,
            PasswordHash = password,
        };

        _context.Users.Add(user);
    }

    public IEnumerable<UserFeed> GetUserFeeds(int userId)
    {
        return _context.UserFeeds.Where(f => f.UserId == userId);
    }

    public void AddUserFeed(int userId, string rssFeed)
    {
        var feed = new UserFeed
        {
            UserId = userId,
            RssFeed = rssFeed,
        };
        
        _context.UserFeeds.Add(feed);
        _context.SaveChanges();
    }

    public void RemoveUserFeed(UserFeed userFeed)
    { 
        _context.UserFeeds.Remove(userFeed); 
        _context.SaveChanges();
    }

    public IEnumerable<UserCargo> GetUserCargos(int userId)
    {
        return _context.UserCargos.Where(c => c.UserId == userId);
    }

    public void SubscribeUserCargo(int userId, int cargoId)
    {
        var cargo = new UserCargo
        {
            UserId = userId,
            CargoId = cargoId,
        };
        _context.UserCargos.Add(cargo);
        _context.SaveChanges();
    }

    public void UnsubscribeUserCargo(int userId, int cargoId)
    {
        var cargo = new UserCargo
        {
            UserId = userId,
            CargoId = cargoId,
        };
        _context.UserCargos.Remove(cargo);
        _context.SaveChanges();
    }
}