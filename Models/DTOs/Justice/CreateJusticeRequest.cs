namespace BmisApi.Models.DTOs.Justice
{
    public record CreateJusticeRequest
        (DateOnly Date, string Complainant, string? ContactInfo, int DefendantId, string Nature, string Status, string Narrative)
    {
    }
}
