using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class ResidentRepository : ICrudRepository<Resident>
    {
        // DbContext DI
        private readonly ApplicationDbContext _context;

        public ResidentRepository(ApplicationDbContext context, IConfiguration config) 
        {
            _context = context;
        }

        public async Task<Resident?> GetByIdAsync(int id)
        {
            return await _context.Residents
                .Include(r => r.Household)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Resident> CreateAsync(Resident entity)
        {
            await _context.Residents.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var resident = await GetByIdAsync(id);

            if (resident is not null)
            {
                resident.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Resident entity)
        {
            _context.Residents.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Resident>> GetAllAsync()
        {
            return await _context.Residents.Include(r => r.Household).ToListAsync();
        }
        
        public async Task<List<Resident>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<Resident>();
            }

            var validIds = ids.Where(id => id > 0).ToList();
            
            var residents = await _context.Residents
                .Include(r => r.Household)
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            var notFoundIds = validIds.Except(residents.Select(r  => r.Id));
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return residents;
        }

        public IQueryable<Resident> Search(string name)
        {
            var query = _context.Residents.AsNoTracking().Include(r => r.Household).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query
                    .Where(e => EF.Functions.ILike(e.FullName, $"%{name}%"));
            }

            return query;
        }
    }
}
