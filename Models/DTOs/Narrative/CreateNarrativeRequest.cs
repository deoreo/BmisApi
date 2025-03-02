namespace BmisApi.Models.DTOs.Narrative
{
    public record CreateNarrativeRequest
        (
        int? BlotterId,
        int? IncidentId,
        int? VawcId,
        string Status,
        string NarrativeReport,
        DateOnly Date
        )
    {
    }
}
