using BmisApi.Models;
using BmisApi.Models.DTOs.Narrative;

namespace BmisApi.Services.NarrativeService
{
    public interface INarrativeService : ICrudService<Narrative, GetNarrativeResponse, GetAllNarrativeResponse, CreateNarrativeRequest, UpdateNarrativeRequest>
    {
    }
}
