namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateBlotterRequest
        (int CaseId, DateOnly Date, int ComplainantId, int DefendantId, string Nature)
    {
    }
}
