namespace BmisApi.Models.DTOs.Incident
{
    public record UpdateIncidentRequest
        (int CaseId, DateOnly Date, int ComplainantId, string Nature, string Narrative)
    {
    }
}
