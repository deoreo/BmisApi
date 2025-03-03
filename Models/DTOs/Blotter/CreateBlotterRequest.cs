namespace BmisApi.Models.DTOs.Blotter
{
    public record CreateBlotterRequest
        (DateOnly Date, string Complainant, string? ContactInfo, int DefendantId, string Nature, string Status, string Narrative)
    {
    }
}
