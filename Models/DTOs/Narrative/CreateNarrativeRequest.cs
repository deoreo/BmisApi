namespace BmisApi.Models.DTOs.Narrative
{
    public record CreateNarrativeRequest
        (
        int? BlotterId,
        int? VawcId,
        string Status,
        string NarrativeReport,
        DateOnly Date
        )
    {
    }
}
