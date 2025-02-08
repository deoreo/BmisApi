namespace BmisApi.Models.DTOs.Incident
{
    public record CreateIncidentRequest
        (int CaseId, DateOnly Date, int ComplainantId, string Nature, string Narrative)
    {
    }
}
