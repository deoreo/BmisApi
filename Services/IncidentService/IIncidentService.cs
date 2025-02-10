using BmisApi.Models;
using BmisApi.Models.DTOs.Incident;

namespace BmisApi.Services.IncidentService
{
    public interface IIncidentService : ICrudService<Incident, GetIncidentResponse, GetAllIncidentResponse, CreateIncidentRequest, UpdateIncidentRequest>
    {
        Task<string?> UpdatePictureAsync(int id, IFormFile picture);
        Task DeletePictureAsync(int id);
    }
}
