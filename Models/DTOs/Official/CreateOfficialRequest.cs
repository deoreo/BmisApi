namespace BmisApi.Models.DTOs.Blotter
{
    public record CreateOfficialRequest
        (string Position, string Title, int ResidentId, DateOnly TermStart, DateOnly TermEnd)
    {
    }
}
