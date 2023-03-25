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
}