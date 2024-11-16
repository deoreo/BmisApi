using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class ResidentRepository : IResidentRepository
    {
        // DbContext DI
        private readonly ApplicationDbContext _context;
        public ResidentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resident?> GetResidentByIdAsync(int id)
        {
            return await _context.Residents
                .Include(r => r.Household)
                .FirstOrDefaultAsync(x => x.ResidentId == id);
        }

        public async Task<Resident> CreateResidentAsync(Resident resident)
        {
            await _context.Residents.AddAsync(resident);
            resident.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return resident;
        }

        public async Task DeleteResidentAsync(int id)
        {
            var resident = await GetResidentByIdAsync(id);

            if (resident is not null)
            {
                resident.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateResidentAsync(Resident resident)
        {
            if (resident.HouseholdId is not null)
            {
                await SetResidentHouseholdAsync(resident);
            }
            _context.Residents.Update(resident);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Resident> GetAllResidentsAsync()
        {
            return _context.Residents.Include(r => r.Household).AsNoTracking();
        }

        public async Task SetResidentHouseholdAsync(Resident resident)
        {
            var household = await _context.Households.FindAsync(resident.HouseholdId);
            if (household is not null)
            {
                resident.Household = household;
            }
        }

        public IQueryable<Resident> Search(string name, Sex? sex)
        {
            var query = _context.Residents.Include(r => r.Household).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FullName.Contains(name));
            }

            if (sex is not null)
            {
                query = query.Where(e => e.Sex == sex);
            }

            return query.AsNoTracking();
        }
    }
}
