namespace BmisApi.Models.DTOs.Blotter
{
    public record GetVawcResponse
        (int Id, string CaseId, DateOnly Date, string ComplainantName, string DefendantName, string Nature, string Status, string Narrative, DateTime CreatedAt)
    {
    }
}
