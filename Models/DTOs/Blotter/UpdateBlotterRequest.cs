namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateBlotterRequest
        (DateOnly Date, int ComplainantId, int DefendantId, string Nature, BlotterStatus Status, string Narrative)
    {
    }
}
