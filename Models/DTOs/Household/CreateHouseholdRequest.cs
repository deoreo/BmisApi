namespace BmisApi.Models.DTOs.Household
{
    public record CreateHouseholdRequest (string Address, int HeadId, IEnumerable<int> MembersId)
    {
    }
}
