using BmisApi.Models;

namespace BmisApi.Repositories
{
    public class BlotterRepository : ICrudRepository<Blotter>
    {
        public Task<Blotter> CreateAsync(Blotter entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Blotter>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Blotter?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Blotter>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Blotter> Search(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Blotter entity)
        {
            throw new NotImplementedException();
        }
    }
}
