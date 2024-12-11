using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Repositories;

namespace BmisApi.Services
{
    public class BlotterService : ICrudService<Blotter, GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest>
    {
        private readonly ICrudRepository<Blotter> _repository;

        public BlotterService(ICrudRepository<Blotter> repository)
        {
            _repository = repository;
        }

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

        public GetBlotterResponse SetResponse(Blotter entity)
        {
            throw new NotImplementedException();
        }

        public Task<GetBlotterResponse?> UpdateAsync(UpdateBlotterRequest request, int id)
        {
            throw new NotImplementedException();
        }
    }
}
