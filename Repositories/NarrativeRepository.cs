using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class NarrativeRepository : ICrudRepository<Narrative>
    {
        private readonly ApplicationDbContext _context;

        public NarrativeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Narrative> CreateAsync(Narrative entity)
        {
            await _context.Narratives.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var narrative = await GetByIdAsync(id);

            if (narrative is not null)
            {
                narrative.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Narrative>> GetAllAsync()
        {
            return await _context.Narratives.AsNoTracking().ToListAsync();
        }

        public async Task<Narrative?> GetByIdAsync(int id)
        {
            return await _context.Narratives.FindAsync(id);
        }

        public async Task<List<Narrative>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<Narrative>();
            }

            var validIds = ids.Where(id => id > 0).ToList();

            var narratives = await _context.Narratives
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            var notFoundIds = validIds.Except(narratives.Select(r => r.Id));
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return narratives;
        }

        public IQueryable<Narrative> Search(string name)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Narrative entity)
        {
            _context.Narratives.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
