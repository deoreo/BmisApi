namespace BmisApi.Models.DTOs.Blotter
{
    public record UpdateOfficialRequest
        (string Position, string Title, int ResidentId, DateOnly TermStart, DateOnly TermEnd)
    {
    }
}
