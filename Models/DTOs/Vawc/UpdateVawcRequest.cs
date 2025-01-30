namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateVawcRequest
        (DateOnly Date, int ComplainantId, int DefendantId, string Nature, VawcStatus Status, string Narrative)
    {
    }
}
