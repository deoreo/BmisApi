using BmisApi.Models;
using System.Collections;

namespace BmisApi.Repositories
{
    public interface ICrudRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetManyByIdAsync(IEnumerable<int> ids);
        IQueryable<TEntity> Search(string name);
    }
}
