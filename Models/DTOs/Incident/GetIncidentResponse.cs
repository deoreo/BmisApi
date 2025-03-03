namespace BmisApi.Models.DTOs.Incident
{
    public record GetIncidentResponse
        (int Id, string CaseId, DateOnly Date, string ComplainantName, string Nature, string Narrative, string? PicturePath, DateTime CreatedAt)
    {
    }
}
