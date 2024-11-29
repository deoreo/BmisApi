namespace BmisApi.Models.DTOs.Household
{
    public record UpdateHouseholdRequest (int? newHeadId, IEnumerable<int>? membersToAdd, IEnumerable<int>? membersToRemove)
    {
    }
}
