namespace BmisApi.Models.DTOs.Narrative
{
    public record UpdateNarrativeRequest
        (
        string Status,
        string NarrativeReport,
        DateOnly Date
        )
    {
    }
}
