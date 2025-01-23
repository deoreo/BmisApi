using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;

namespace BmisApi.Services.OfficialService
{
    public interface IOfficialService : ICrudService<Official, GetOfficialResponse, GetAllOfficialResponse, CreateOfficialRequest, UpdateOfficialRequest>
    {
    }
}
