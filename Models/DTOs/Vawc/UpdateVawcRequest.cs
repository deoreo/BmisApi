namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateVawcRequest
        (string Complainant, string? ContactInfo, int DefendantId, string Nature)
    {
    }
}
