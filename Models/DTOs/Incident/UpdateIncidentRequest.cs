namespace BmisApi.Models.DTOs.Incident
{
    public record UpdateIncidentRequest
        (DateOnly Date, int ComplainantId, string Nature, string Narrative)
    {
    }
}
