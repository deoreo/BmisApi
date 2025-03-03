using BmisApi.Models;
using BmisApi.Models.DTOs.Justice;
using BmisApi.Models.DTOs.Narrative;

namespace BmisApi.Services.JusticeService
{
    public interface IJusticeService : ICrudService<Justice, GetJusticeResponse, GetAllJusticeResponse, CreateJusticeRequest, UpdateJusticeRequest>
    {
        Task<GetAllNarrativeResponse> GetNarrativesAsync(int id);
    }
}
