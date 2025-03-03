namespace BmisApi.Models.DTOs.Incident
{
    public record CreateIncidentRequest
        (DateOnly Date, List<ComplainantInfo> Complainants, string Nature, string Narrative)
    {
    }
}
