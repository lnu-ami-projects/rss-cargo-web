namespace RSSCargo.DAL.Repositories.Contracts;

using Models;

public interface IGenericRepository<TModel> where TModel: class
{
    public User? GetUserByEmail(string email);
}