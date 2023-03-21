namespace RSSCargo.DAL.Repositories;

using DataContext;
using Contracts;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
{
    private readonly RsscargoContext _dbContext;

    public GenericRepository(RsscargoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TModel>> GetUsers()
    {
        try
        {
            return await _dbContext.Set<TModel>().ToListAsync();
        }
        catch
        {
            throw new Exception();
        }
    }
}