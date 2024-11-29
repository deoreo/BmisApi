namespace BmisApi.Models.DTOs.Household
{
    public record GetHouseholdResponse (int householdId, string address, int memberCount, string headName,
        int age, Sex sex, DateOnly birthday, string? occupation, DateTime createdAt)
    {
    }
}
