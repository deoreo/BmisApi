namespace BmisApi.Models.DTOs.Blotter
{
    public record CreateBlotterRequest
        (int CaseId, DateOnly Date, int ComplainantId, int DefendantId, string Nature, string Status, string Narrative)
    {
    }
}
