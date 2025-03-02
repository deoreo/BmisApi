using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Narrative;

namespace BmisApi.Services.VawcService
{
    public interface IVawcService : ICrudService<Vawc, GetVawcResponse, GetAllVawcResponse, CreateVawcRequest, UpdateVawcRequest>
    {
        Task<GetAllNarrativeResponse> GetNarrativesAsync(int id);
    }
}
