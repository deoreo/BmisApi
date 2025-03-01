namespace BmisApi.Models.DTOs.Narrative
{
    public record CreateNarrativeRequest
        (
        int ReportId,
        string Status,
        string NarrativeReport,
        DateOnly Date
        )
    {
    }
}
