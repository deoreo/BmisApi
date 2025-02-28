namespace BmisApi.Models.DTOs.Household
{
    public record UpdateHouseholdRequest (int? NewHeadId, IEnumerable<int>? MembersToAdd, IEnumerable<int>? MembersToRemove, string Address)
    {
    }
}
