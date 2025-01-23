using BmisApi.Models;
using BmisApi.Models.DTOs.Household;
using BmisApi.Models.DTOs.Resident;

namespace BmisApi.Services.HouseholdService
{
    public interface IHouseholdService : ICrudService<Household, GetHouseholdResponse, GetAllHouseholdResponse, CreateHouseholdRequest, UpdateHouseholdRequest>
    {
        Task<GetAllResidentResponse> GetMembersAsync(int id);
    }
}
