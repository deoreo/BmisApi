namespace BmisApi.Models.DTOs.Incident
{
    public record UpdateIncidentRequest
        (List<IncidentComplainant> Complainants, string Nature, string Narrative)
    {
    }
}
