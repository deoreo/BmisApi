using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class IncidentRepository : ICrudRepository<Incident>
    {
        private readonly ApplicationDbContext _context;

        public IncidentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incident?> GetByIdAsync(int id)
        {
            return await _context.Incidents
                .Include(i => i.Complainants)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Incident> CreateAsync(Incident entity)
        {
            await _context.Incidents.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var incident = await GetByIdAsync(id);

            if (incident is not null)
            {
                incident.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Incident entity)
        {
            _context.Incidents.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Incident>> GetAllAsync()
        {
            return await _context.Incidents.AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Incident>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<Incident>();
            }

            var validIds = ids.Where(id => id > 0).ToList();

            var incidents = await _context.Incidents
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            var notFoundIds = validIds.Except(incidents.Select(r => r.Id));
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return incidents;
        }

        public IQueryable<Incident> Search(string name)
        {
            throw new NotImplementedException();
        }

        
    }
}
