using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Models.DTOs.Narrative;

namespace BmisApi.Services.BlotterService
{
    public interface IBlotterService : ICrudService<Blotter, GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest>
    {
        Task<GetAllNarrativeResponse> GetNarrativesAsync(int id);
    }
}
