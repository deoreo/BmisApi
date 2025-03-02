namespace BmisApi.Models.DTOs.Incident
{
    public record UpdateIncidentRequest
        (int ComplainantId, string Nature)
    {
    }
}
