namespace BmisApi.Models.DTOs.Blotter
{
    public record CreateBlotterRequest
        (DateOnly Date, int ComplainantId, int DefendantId, string Nature, Status Status, string Narrative)
    {
    }
}
