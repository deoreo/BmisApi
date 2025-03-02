namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateBlotterRequest
        (int ComplainantId, int DefendantId, string Nature)
    {
    }
}
