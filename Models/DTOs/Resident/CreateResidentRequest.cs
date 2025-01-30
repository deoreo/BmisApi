namespace BmisApi.Models.DTOs.Resident
{
    public record CreateResidentRequest
        (string FirstName, string? MiddleName, string LastName, string? Suffix, Sex Sex, DateOnly Birthday,
        string? Occupation, bool RegisteredVoter)
    {
    }
}