namespace BmisApi.Models.DTOs.BrgyProject
{
    public record class GetBrgyProjectResponse
        (int Id, string ReferenceCode, string ProjectDescription, string ImplementingAgency, DateOnly StartingDate, DateOnly CompletionDate,
        string ExpectedOutput, string FundingSource, decimal? PS, decimal? MOE, decimal? CO, decimal Total, DateTime CreatedAt)
    {
    }
}
