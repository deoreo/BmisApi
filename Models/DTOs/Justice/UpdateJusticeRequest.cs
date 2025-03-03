namespace BmisApi.Models.DTOs.Justice
{
    public record UpdateJusticeRequest
        (string Complainant, string? ContactInfo, int DefendantId, string Nature)
    {
    }
}
