using BmisApi.Models.DTOs.Resident;
using BmisApi.Models;

namespace BmisApi.Services.ResidentService.ResidentService
{
    public interface IResidentService : ICrudService<Resident, GetResidentResponse, GetAllResidentResponse, CreateResidentRequest, UpdateResidentRequest>
    {
        Task<string?> UpdatePictureAsync(int id, IFormFile picture);
        Task DeletePictureAsync(int id);
    }
}
