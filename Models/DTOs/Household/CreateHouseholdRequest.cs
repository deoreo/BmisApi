namespace BmisApi.Models.DTOs.Household
{
    public record CreateHouseholdRequest (string address, int headId, IEnumerable<int> membersId)
    {
    }
}
