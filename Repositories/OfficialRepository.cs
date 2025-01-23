using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class OfficialRepository : ICrudRepository<Official>
    {
        private readonly ApplicationDbContext _context;

        public OfficialRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Official?> GetByIdAsync(int id)
        {
            return await _context.Officials
                .Include(o => o.Resident)
                .FirstOrDefaultAsync();
        }

        public async Task<Official> CreateAsync(Official entity)
        {
            await _context.Officials.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var official = await GetByIdAsync(id);

            if (official is not null)
            {
                official.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Official entity)
        {
            _context.Officials.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Official>> GetAllAsync()
        {
            return await _context.Officials
                .Include(o => o.Resident)
                .ToListAsync();
        }

        

        public Task<List<Official>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Official> Search(string name)
        {
            throw new NotImplementedException();
        }

       
    }
}
