namespace BmisApi.Models.DTOs
{
    public record CreateResidentRequest
        (string FullName, Sex Sex, DateOnly Birthday,
        string? Occupation, bool RegisteredVoter)
    {
    }
}