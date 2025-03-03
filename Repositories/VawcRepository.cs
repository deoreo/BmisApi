using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class VawcRepository : ICrudRepository<Vawc>
    {
        private readonly ApplicationDbContext _context;

        public VawcRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vawc?> GetByIdAsync(int id)
        {
            return await _context.Vawcs
                .Include(b => b.Defendant)
                .Include(b => b.NarrativeReports)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Vawc> CreateAsync(Vawc entity)
        {
            await _context.Vawcs.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var vawc = await GetByIdAsync(id);

            if (vawc is not null)
            {
                vawc.DeletedAt = DateTime.UtcNow;
                foreach (var narrative in vawc.NarrativeReports)
                {
                    narrative.DeletedAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Vawc entity)
        {
            _context.Vawcs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Vawc>> GetAllAsync()
        {
            return await _context.Vawcs
                .Include(b => b.Defendant)
                .ToListAsync();
        }

        public async Task<List<Vawc>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<Vawc>();
            }

            var validIds = ids.Where(id => id > 0).ToList();

            var vawc = await _context.Vawcs.AsNoTracking()
                .Include(r => r.Defendant)
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            var notFoundIds = validIds.Except(vawc.Select(r => r.Id));
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return vawc;
        }

        public IQueryable<Vawc> Search(string name)
        {
            throw new NotImplementedException();
        }

    }
}
