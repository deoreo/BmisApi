namespace BmisApi.Models.DTOs.Incident
{
    public record CreateIncidentRequest
        (DateOnly Date, int ComplainantId, string Nature, string Narrative)
    {
    }
}
