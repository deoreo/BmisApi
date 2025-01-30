using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;

namespace BmisApi.Services.VawcService
{
    public interface IVawcService : ICrudService<Vawc, GetVawcResponse, GetAllVawcResponse, CreateVawcRequest, UpdateVawcRequest>
    {
    }
}
