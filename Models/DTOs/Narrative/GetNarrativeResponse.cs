namespace BmisApi.Models.DTOs.Narrative
{
    public record GetNarrativeResponse
        (
        int ReportId,
        string Status,
        string NarrativeReport,
        DateOnly Date,
        DateTime CreatedAt
        )
    {
    }
}
