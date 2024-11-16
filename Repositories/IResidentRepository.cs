using BmisApi.Models;

namespace BmisApi.Repositories
{
    public interface IResidentRepository
    {
        Task<Resident?> GetResidentByIdAsync(int id);
        Task<Resident> CreateResidentAsync(Resident resident);
        Task DeleteResidentAsync(int id);
        Task UpdateResidentAsync(Resident resident);
        IQueryable<Resident> GetAllResidentsAsync();
        Task SetResidentHouseholdAsync(Resident resident);
        IQueryable<Resident> Search(string name, Sex? sex);
    }
}
