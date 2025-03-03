namespace BmisApi.Models.DTOs.Narrative
{
    public record CreateNarrativeRequest
        (
        int? BlotterId,
        int? VawcId,
        int? JusticeId,
        string Status,
        string NarrativeReport,
        DateOnly Date
        )
    {
    }
}
