namespace BmisApi.Models.DTOs.Resident
{
    public record UpdateResidentRequest
        (string FullName, Sex Sex, DateOnly Birthday,
        string? Occupation, bool RegisteredVoter)
    {

    }
}
