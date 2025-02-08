namespace BmisApi.Models.DTOs.Blotter
{
    public record CreateVawcRequest
        (int CaseId, DateOnly Date, int ComplainantId, int DefendantId, string Nature, VawcStatus Status, string Narrative)
    {
    }
}
