namespace RSSCargo.DAL.Repositories;

using DataContext;
using Contracts;
using Models;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<IEntityBase> : RssCargoContext, IGenericRepository<IEntityBase> where IEntityBase : class
{
    public User? GetUserByEmail(string email)
    {
        return Users.FirstOrDefault(u => u.Email == email);
    }
    
}