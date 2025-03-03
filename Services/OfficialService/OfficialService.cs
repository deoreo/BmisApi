using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Repositories;
using System.Globalization;

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
                throw new KeyNotFoundException($"Official with ID {id} not found");
            }

            return SetResponse(official);
        }

        public async Task<GetOfficialResponse> CreateAsync(CreateOfficialRequest request)
        {
            var resident = await _residentRepository.GetByIdAsync(request.ResidentId);
            if (resident == null)
            {
                throw new KeyNotFoundException($"Provided resident with id {request.ResidentId} not found");
            }

            var allOfficials = await _officialRepository.GetAllAsync();

            var existingOfficial = allOfficials.FirstOrDefault(o => o.ResidentId == request.ResidentId);
            if (existingOfficial != null)
            {
                throw new InvalidOperationException($"Resident with id {request.ResidentId} already holds the position of {existingOfficial.Position}");
            }

            if (request.Position.ToLower() == "barangay captain" && allOfficials.Any(o => o.Position.ToLower() == "barangay captain"))
            {
                throw new InvalidOperationException("Only one captain is allowed.");
            }
            if (request.Position.ToLower() == "secretary" && allOfficials.Count(o => o.Position.ToLower() == "secretary") >= 1)
            {
                throw new InvalidOperationException("Only one secretary is allowed.");
            }
            if (request.Position.ToLower() == "treasurer" && allOfficials.Count(o => o.Position.ToLower() == "treasurer") >= 1)
            {
                throw new InvalidOperationException("Only one treasurer is allowed.");
            }
            if (request.Position.ToLower() == "sk chairman" && allOfficials.Count(o => o.Position.ToLower() == "sk chairman") >= 1)
            {
                throw new InvalidOperationException("Only one SK chairman is allowed.");
            }
            if (request.Position.ToLower() == "councilor" && allOfficials.Count(o => o.Position.ToLower() == "councilor") >= 7)
            {
                throw new InvalidOperationException("Only seven councilors are allowed.");
            }

            var official = new Official
            {
                Position = request.Position,
                Title = ToTitleCase(request.Title).Trim(),
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
                throw new KeyNotFoundException($"Provided resident with id {request.ResidentId} not found");
            }

            var official = await _officialRepository.GetByIdAsync(id);
            if (official == null)
            {
                throw new InvalidOperationException($"Provided official with id {id} not found");
            }

            var allOfficials = await _officialRepository.GetAllAsync();

            var existingOfficial = allOfficials.FirstOrDefault(o => o.ResidentId == request.ResidentId);
            if (existingOfficial != null)
            {
                throw new InvalidOperationException($"Resident with id {request.ResidentId} already holds the position of {existingOfficial.Position}");
            }

            if (request.Position.ToLower() == "barangay captain" && allOfficials.Any(o => o.Position.ToLower() == "barangay captain"))
            {
                throw new InvalidOperationException("Only one captain is allowed.");
            }
            if (request.Position.ToLower() == "secretary" && allOfficials.Count(o => o.Position.ToLower() == "secretary") >= 1)
            {
                throw new InvalidOperationException("Only one secretary is allowed.");
            }
            if (request.Position.ToLower() == "treasurer" && allOfficials.Count(o => o.Position.ToLower() == "treasurer") >= 1)
            {
                throw new InvalidOperationException("Only one treasurer is allowed.");
            }
            if (request.Position.ToLower() == "sk chairman" && allOfficials.Count(o => o.Position.ToLower() == "sk chairman") >= 1)
            {
                throw new InvalidOperationException("Only one SK chairman is allowed.");
            }
            if (request.Position.ToLower() == "councilor" && allOfficials.Count(o => o.Position.ToLower() == "councilor") >= 7)
            {
                throw new InvalidOperationException("Only seven councilors are allowed.");
            }

            official.Position = request.Position;
            official.Title = ToTitleCase(request.Title).Trim();
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

        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }
    }
}
