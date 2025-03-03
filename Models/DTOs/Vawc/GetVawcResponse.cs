namespace BmisApi.Models.DTOs.Blotter
{
    public record GetVawcResponse
        (int Id,
        string CaseId, 
        DateOnly Date, 
        string Complainant, 
        string? ContactInfo, 
        string DefendantName, 
        string Nature, 
        string Status, 
        DateTime CreatedAt)
    {
    }
}
