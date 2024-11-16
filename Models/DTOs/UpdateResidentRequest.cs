namespace BmisApi.Models.DTOs
{
    public record UpdateResidentRequest
        (string FullName, Sex Sex, DateOnly Birthday,
        string? Occupation, bool RegisteredVoter, int? HouseholdId)
    {
        
    }
}
