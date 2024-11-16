namespace BmisApi.Models.DTOs
{
    public record GetResidentResponse
        (int ResidentId, string FullName, int Age, Sex Sex, DateOnly Birthday, 
        string? Occupation, bool RegisteredVoter, int? HouseholdId, string? Address, DateTime CreatedAt)
    {
    }
}
