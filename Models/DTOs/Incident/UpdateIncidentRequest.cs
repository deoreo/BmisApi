namespace BmisApi.Models.DTOs.Incident
{
    public record UpdateIncidentRequest
        (List<ComplainantInfo> Complainants, string Nature, string Narrative)
    {
    }
}
