using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class JusticeRepository : ICrudRepository<Justice>
    {
        private readonly ApplicationDbContext _context;

        public JusticeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Justice?> GetByIdAsync(int id)
        {
            return await _context.Justices
                .Include(b => b.Defendant)
                .Include(b => b.NarrativeReports)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Justice> CreateAsync(Justice entity)
        {
            await _context.Justices.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var justice = await GetByIdAsync(id);

            if (justice is not null)
            {
                justice.DeletedAt = DateTime.UtcNow;
                foreach (var narrative in justice.NarrativeReports)
                {
                    narrative.DeletedAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Justice entity)
        {
            _context.Justices.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Justice>> GetAllAsync()
        {
            return await _context.Justices.AsNoTracking()
                .Include(b => b.Defendant)
                .ToListAsync();
        }

        public async Task<List<Justice>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<Justice>();
            }

            var validIds = ids.Where(id => id > 0).ToList();

            var justices = await _context.Justices
                .Include(r => r.Defendant)
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            var notFoundIds = validIds.Except(justices.Select(r => r.Id));
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return justices;
        }

        public IQueryable<Justice> Search(string name)
        {
            throw new NotImplementedException();
        }
    }
}
