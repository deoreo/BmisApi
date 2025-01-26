using BmisApi.Models.DTOs.Blotter;

namespace BmisApi.Models.DTOs.Incident
{
    public record GetAllIncidentResponse (List<GetIncidentResponse> Incidents)
    {
    }
}
