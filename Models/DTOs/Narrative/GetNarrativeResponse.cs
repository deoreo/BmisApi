namespace BmisApi.Models.DTOs.Narrative
{
    public record GetNarrativeResponse
        (
        int Id,
        string CaseId,
        int? ReportId,
        string Status,
        string NarrativeReport,
        DateOnly Date,
        DateTime CreatedAt
        )
    {
    }
}
