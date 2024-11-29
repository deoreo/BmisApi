namespace BmisApi.Models.DTOs.Resident
{
    public record CreateResidentRequest
        (string FullName, Sex Sex, DateOnly Birthday,
        string? Occupation, bool RegisteredVoter)
    {
    }
}