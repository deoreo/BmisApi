using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class HouseholdRepository : ICrudRepository<Household>
    {
        private readonly ApplicationDbContext _context;

        public HouseholdRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Household?> GetByIdAsync(int id)
        {
            return await _context.Households
                .Include(h => h.Members)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Household> CreateAsync(Household entity)
        {
            await _context.Households.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var household = await GetByIdAsync(id);

            if (household is not null)
            {
                household.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Household entity)
        {
            _context.Households.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Household>> GetAllAsync()
        {
            return await _context.Households.AsNoTracking()
                .Include(h => h.Members)
                .ToListAsync();
        }

        public Task<List<Household>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
        public IQueryable<Household> Search(string name)
        {
            throw new NotImplementedException();
        }
    }
}
