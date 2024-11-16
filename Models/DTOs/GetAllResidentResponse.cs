namespace BmisApi.Models.DTOs
{
    public record GetAllResidentResponse(IEnumerable<GetResidentResponse> residents)
    {
    }
}
