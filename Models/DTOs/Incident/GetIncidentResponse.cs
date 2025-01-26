namespace BmisApi.Models.DTOs.Incident
{
    public record GetIncidentResponse
        (int Id, DateOnly Date, string ComplainantName, string Nature, string Narrative, DateTime CreatedAt)
    {
    }
}
