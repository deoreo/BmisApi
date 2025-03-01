using BmisApi.Services;
using System.Text.Json.Serialization;

namespace BmisApi.Models.DTOs.Blotter
{
    public record GetBlotterResponse
        (int Id, 
        int CaseId,
        DateOnly Date, 
        string ComplainantName, 
        string DefendantName, 
        string Nature, 
        string Status, 
        DateTime CreatedAt)
    {
    }
}
