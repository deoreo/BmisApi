using BmisApi.Models;
using BmisApi.Models.DTOs.Incident;

namespace BmisApi.Services.IncidentService
{
    public interface IIncidentService : ICrudService<Incident, GetIncidentResponse, GetAllIncidentResponse, CreateIncidentRequest, UpdateIncidentRequest>
    {
    }
}
