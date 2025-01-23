using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Repositories;

namespace BmisApi.Services.OfficialService
{
    public class OfficialService : IOfficialService
    {
        private readonly ICrudRepository<Official> _officialRepository;
        private readonly ICrudRepository<Resident> _residentRepository;

        public OfficialService(ICrudRepository<Official> officialRepository, ICrudRepository<Resident> residentRepository)
        {
            _officialRepository = officialRepository;
            _residentRepository = residentRepository;
        }

        public async Task<GetOfficialResponse?> GetByIdAsync(int id)
        {
            var official = await _officialRepository.GetByIdAsync(id);
            if (official == null)
            {
                return null;
            }

            return SetResponse(official);
        }

        public async Task<GetOfficialResponse> CreateAsync(CreateOfficialRequest request)
        {
            var resident = await _residentRepository.GetByIdAsync(request.ResidentId);
            if (resident == null)
            {
                throw new Exception($"Provided resident with id {request.ResidentId} not found");
            }

            var official = new Official
            {
                Position = request.Position,
                Title = request.Title,
                ResidentId = request.ResidentId,
                Resident = resident,
                TermStart = request.TermStart,
                TermEnd = request.TermEnd
            };

            official = await _officialRepository.CreateAsync(official);

            return SetResponse(official);
        }

        public async Task DeleteAsync(int id)
        {
            await _officialRepository.DeleteAsync(id);
        }

        public async Task<GetOfficialResponse?> UpdateAsync(UpdateOfficialRequest request, int id)
        {
            var newResident = await _residentRepository.GetByIdAsync(request.ResidentId);
            if (newResident == null)
            {
                throw new Exception($"Provided resident with id {request.ResidentId} not found");
            }

            var official = await _officialRepository.GetByIdAsync(id);
            if (official == null)
            {
                throw new Exception($"Provided official with id {id} not found");
            }

            official.Position = request.Position;
            official.Title = request.Title;
            official.ResidentId = request.ResidentId;
            official.Resident = newResident;
            official.TermStart = request.TermStart;
            official.TermEnd = request.TermEnd;
            official.LastUpdatedAt = DateTime.UtcNow;

            await _officialRepository.UpdateAsync(official);

            return SetResponse(official);

        }

        public async Task<GetAllOfficialResponse> GetAllAsync()
        {
            var officials = await _officialRepository.GetAllAsync();

            var responses = officials.Select(SetResponse).ToList();

            return new GetAllOfficialResponse(responses);
        }

        

        public Task<GetAllOfficialResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetOfficialResponse SetResponse(Official official)
        {
            var response = new GetOfficialResponse
                (
                official.Id,
                official.Position,
                official.Title,
                official.Resident.FullName,
                official.TermStart,
                official.TermEnd,
                official.CreatedAt
                );

            return response;
        }
    }
}
