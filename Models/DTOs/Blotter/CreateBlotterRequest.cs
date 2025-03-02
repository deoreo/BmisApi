namespace BmisApi.Models.DTOs.Blotter
{
    public record CreateBlotterRequest
        (DateOnly Date, int ComplainantId, int DefendantId, string Nature, string Status, string Narrative)
    {
    }
}
