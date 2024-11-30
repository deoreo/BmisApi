using BmisApi.Models.DTOs.BrgyProject;

namespace BmisApi.Services
{
    public class BrgyProjectService : ICrudService<GetBrgyProjectResponse, GetAllBrgyProjectResponse, CreateBrgyProjectRequest, UpdateBrgyProjectRequest>
    {
        public Task<GetBrgyProjectResponse> CreateAsync(CreateBrgyProjectRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllBrgyProjectResponse> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetBrgyProjectResponse?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllBrgyProjectResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public Task<GetBrgyProjectResponse?> UpdateAsync(UpdateBrgyProjectRequest request, int id)
        {
            throw new NotImplementedException();
        }
    }
}
