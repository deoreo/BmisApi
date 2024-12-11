using BmisApi.Models;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Services
{
    public class ResidentService : ICrudService<Resident, GetResidentResponse, GetAllResidentResponse, CreateResidentRequest, UpdateResidentRequest>
    {
        private readonly ICrudRepository<Resident> _repository;

        public ResidentService(ICrudRepository<Resident> repository)
        {
            _repository = repository;
        }

        public async Task<GetResidentResponse?> GetByIdAsync(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null)
            {
                return null;
            }

            return SetResponse(resident);
        }

        public async Task<GetResidentResponse> CreateAsync(CreateResidentRequest request)
        {
            var resident = new Resident
                (
                    request.FullName,
                    request.Sex,
                    request.Birthday,
                    request.Occupation,
                    request.RegisteredVoter
                );

            resident = await _repository.CreateAsync(resident);

            return SetResponse(resident);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<GetResidentResponse?> UpdateAsync(UpdateResidentRequest request, int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null)
            {
                return null;
            }

            resident.FullName = request.FullName;
            resident.Sex = request.Sex;
            resident.Birthday = request.Birthday;
            resident.Occupation = request.Occupation;
            resident.RegisteredVoter = request.RegisteredVoter;
            resident.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(resident);

            return SetResponse(resident);
        }

        public async Task<GetAllResidentResponse> GetAllAsync()
        {
            var residents = await _repository.GetAllAsync();
            
            var residentResponse = residents.Select(SetResponse).ToList();

            return new GetAllResidentResponse(residentResponse);
        }

        public async Task<GetAllResidentResponse> Search(string name)
        {
            var result = await _repository.Search(name).ToListAsync();
            var residentResponse = new List<GetResidentResponse>();

            foreach (var resident in result)
            {
                residentResponse.Add(SetResponse(resident));
            }

            return new GetAllResidentResponse(residentResponse);
        }

        public GetResidentResponse SetResponse(Resident resident)
        {
            var response = new GetResidentResponse
                (
                resident.Id,
                resident.FullName,
                resident.GetAge(resident.Birthday),
                resident.Sex.ToString(),
                resident.Birthday,
                resident.Occupation,
                resident.RegisteredVoter,
                resident.HouseholdId,
                resident.Household?.Address,
                resident.CreatedAt
                );

            return response;
        }
    }
}
