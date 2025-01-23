namespace BmisApi.Models.DTOs.Blotter
{
    public record GetOfficialResponse
        (int Id, string Position, string? Title, string OfficialName, DateOnly TermStart, DateOnly TermEnd, DateTime CreatedAt)
    {
    }
}
