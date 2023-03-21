namespace RSSCargo.DAL.Repositories.Contracts;

public interface IGenericRepository<TModel> where TModel: class
{
    Task<List<TModel>> GetUsers();
}