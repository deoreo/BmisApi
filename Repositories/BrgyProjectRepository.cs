using BmisApi.Models;

namespace BmisApi.Repositories
{
    public class BrgyProjectRepository : ICrudRepository<BrgyProject>
    {
        public Task<BrgyProject> CreateAsync(BrgyProject entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BrgyProject>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BrgyProject?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BrgyProject>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BrgyProject> Search(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BrgyProject entity)
        {
            throw new NotImplementedException();
        }
    }
}
