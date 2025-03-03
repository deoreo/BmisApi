namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateBlotterRequest
        (string Complainant, string? ContactInfo, int DefendantId, string Nature)
    {
    }
}
