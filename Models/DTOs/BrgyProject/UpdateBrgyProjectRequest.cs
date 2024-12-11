namespace BmisApi.Models.DTOs.BrgyProject
{
    public record class UpdateBrgyProjectRequest
        (string ReferenceCode, string ImplementingAgency, DateOnly StartingDate, DateOnly CompletionDate,
        string ExpectedOutput, string FundingSource, decimal? PS, decimal? MOE, decimal? CO)
    {
    }
}
