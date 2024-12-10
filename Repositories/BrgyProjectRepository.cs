using BmisApi.Data;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Repositories
{
    public class BrgyProjectRepository : ICrudRepository<BrgyProject>
    {
        private readonly ApplicationDbContext _context;

        public BrgyProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BrgyProject?> GetByIdAsync(int id)
        {
            return await _context.BrgyProjects.FindAsync(id);
        }
        public async Task<BrgyProject> CreateAsync(BrgyProject entity)
        {
            await _context.BrgyProjects.AddAsync(entity);
            entity.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var brgyProject = await GetByIdAsync(id);

            if (brgyProject is not null)
            {
                brgyProject.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(BrgyProject entity)
        {
            _context.BrgyProjects.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BrgyProject>> GetAllAsync()
        {
            return await _context.BrgyProjects.ToListAsync();
        }

        public async Task<List<BrgyProject>> GetManyByIdAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<BrgyProject>();
            }

            var validIds = ids.Where(id => id > 0).ToList();

            var brgyProjects = await _context.BrgyProjects
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            var notFoundIds = validIds.Except(brgyProjects.Select(r => r.Id));
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return brgyProjects;
        }

        public IQueryable<BrgyProject> Search(string name)
        {
            throw new NotImplementedException();
        }

        
    }
}
