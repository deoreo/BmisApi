namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateVawcRequest
        (int ComplainantId, int DefendantId, string Nature)
    {
    }
}
