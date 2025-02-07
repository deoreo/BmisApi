using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class BlotterRepository : ICrudRepository<Blotter>
    {
        private readonly ApplicationDbContext _context;

        public BlotterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Blotter?> GetByIdAsync(int id)
        {
            return await _context.Blotters
                .Include(b => b.Complainant)
                .Include(b => b.Defendant)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Blotter> CreateAsync(Blotter entity)
        {
            await _context.Blotters.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var blotter = await GetByIdAsync(id);

            if (blotter is not  null)
            {
                blotter.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Blotter entity)
        {
            _context.Blotters.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Blotter>> GetAllAsync()
        {
            return await _context.Blotters.AsNoTracking()
                .Include(b => b.Complainant)
                .Include(b => b.Defendant)
                .ToListAsync();
        }

        public async Task<List<Blotter>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<Blotter>();
            }

            var validIds = ids.Where(id => id > 0).ToList();

            var blotters = await _context.Blotters
                .Include(r => r.Complainant)
                .Include(r => r.Defendant)
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            var notFoundIds = validIds.Except(blotters.Select(r => r.Id));
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return blotters;
        }

        public IQueryable<Blotter> Search(string name)
        {
            throw new NotImplementedException();
        }

        
    }
}
