using BmisApi.Models.DTOs.Blotter;

namespace BmisApi.Services
{
    public class BlotterService : ICrudService<GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest>
    {
        public Task<GetBlotterResponse> CreateAsync(CreateBlotterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllBlotterResponse> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetBlotterResponse?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllBlotterResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public Task<GetBlotterResponse?> UpdateAsync(UpdateBlotterRequest request, int id)
        {
            throw new NotImplementedException();
        }
    }
}
