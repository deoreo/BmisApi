namespace BmisApi.Models.DTOs.Household
{
    public record GetHouseholdResponse (int Id, string Address, int MemberCount, string HeadName,
        int Age, Sex Sex, DateOnly Birthday, string? Occupation, DateTime CreatedAt)
    {
    }
}
