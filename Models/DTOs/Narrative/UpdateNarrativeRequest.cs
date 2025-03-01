namespace BmisApi.Models.DTOs.Narrative
{
    public record UpdateNarrativeRequest
        (
        int ReportId,
        string Status,
        string NarrativeReport,
        DateOnly Date
        )
    {
    }
}
