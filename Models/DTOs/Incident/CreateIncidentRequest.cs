namespace BmisApi.Models.DTOs.Incident
{
    public record CreateIncidentRequest
        (DateOnly Date, List<IncidentComplainant> Complainants, string Nature, string Narrative)
    {
    }
}
