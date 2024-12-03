namespace BmisApi.Models.DTOs.Household
{
    public record GetHouseholdResponse (int Id, string address, int memberCount, string headName,
        int age, Sex sex, DateOnly birthday, string? occupation, DateTime createdAt)
    {
    }
}
