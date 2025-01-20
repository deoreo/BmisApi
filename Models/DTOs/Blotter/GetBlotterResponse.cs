namespace BmisApi.Models.DTOs.Blotter
{
    public record GetBlotterResponse
        (DateOnly Date, string ComplainantName, string DefendantName, string Nature, string Status, DateTime CreatedAt)
    {
    }
}
