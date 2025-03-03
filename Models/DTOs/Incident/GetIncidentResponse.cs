namespace BmisApi.Models.DTOs.Incident
{
    public record GetIncidentResponse
        (int Id, string CaseId, DateOnly Date, List<IncidentComplainant> Complainants, string Nature, string Narrative, string? PicturePath, DateTime CreatedAt)
    {
    }
}
