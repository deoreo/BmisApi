using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Repositories;

namespace BmisApi.Services.VawcService
{
    public class VawcService : IVawcService
    {
        private readonly ICrudRepository<Vawc> _vawcRepository;
        private readonly ICrudRepository<Resident> _residentRepository;

        public VawcService(ICrudRepository<Vawc> vawcRepository, ICrudRepository<Resident> residentRepository)
        {
            _vawcRepository = vawcRepository;
            _residentRepository = residentRepository;
        }

        public async Task<GetVawcResponse?> GetByIdAsync(int id)
        {
            var vawc = await _vawcRepository.GetByIdAsync(id);
            if (vawc == null)
            {
                throw new KeyNotFoundException($"VAWC with ID {id} not found");
            }

            return SetResponse(vawc);
        }
        public async Task<GetVawcResponse> CreateAsync(CreateVawcRequest request)
        {
            var complainant = await _residentRepository.GetByIdAsync(request.ComplainantId);
            if (complainant == null)
            {
                throw new KeyNotFoundException($"Provided complainant resident with id {request.ComplainantId} not found");
            }

            var dateNow = DateOnly.FromDateTime(DateTime.Today);
            if (request.Date >= dateNow)
            {
                throw new Exception("Invalid date");
            }

            var defendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (defendant == null)
            {
                throw new KeyNotFoundException($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var vawc = new Vawc
            {
                Date = request.Date,
                CaseId = request.CaseId,
                ComplainantId = request.ComplainantId,
                Complainant = complainant,
                DefendantId = request.DefendantId,
                Defendant = defendant,
                Nature = request.Nature,
                Status = request.Status,
                Narrative = request.Narrative
            };

            vawc = await _vawcRepository.CreateAsync(vawc);

            return SetResponse(vawc);
        }

        public async Task DeleteAsync(int id)
        {
            await _vawcRepository.DeleteAsync(id);
        }

        public async Task<GetVawcResponse?> UpdateAsync(UpdateVawcRequest request, int id)
        {
            var newComplainant = await _residentRepository.GetByIdAsync(request.ComplainantId);
            if (newComplainant == null)
            {
                throw new KeyNotFoundException($"Provided complainant resident with id {request.ComplainantId} not found");
            }

            var newDefendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (newDefendant == null)
            {
                throw new KeyNotFoundException($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var dateNow = DateOnly.FromDateTime(DateTime.Today);
            if (request.Date >= dateNow)
            {
                throw new Exception("Invalid date");
            }

            var vawc = await _vawcRepository.GetByIdAsync(id);
            if (vawc == null)
            {
                throw new KeyNotFoundException($"VAWC with ID {id} not found");
            }

            vawc.Date = request.Date;
            vawc.CaseId = request.CaseId;
            vawc.ComplainantId = request.ComplainantId;
            vawc.Complainant = newComplainant;
            vawc.DefendantId = request.DefendantId;
            vawc.Defendant = newDefendant;
            vawc.Nature = request.Nature;
            vawc.Status = request.Status;
            vawc.Narrative = request.Narrative;
            vawc.LastUpdatedAt = DateTime.UtcNow;

            await _vawcRepository.UpdateAsync(vawc);

            return SetResponse(vawc);
        }

        public async Task<GetAllVawcResponse> GetAllAsync()
        {
            var vawcs = await _vawcRepository.GetAllAsync();

            var responses = vawcs.Select(SetResponse).ToList();

            return new GetAllVawcResponse(responses);
        }

        public Task<GetAllVawcResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetVawcResponse SetResponse(Vawc vawc)
        {
            var response = new GetVawcResponse
                (
                vawc.Id,
                vawc.CaseId,
                vawc.Date,
                vawc.Complainant.FullName,
                vawc.Defendant.FullName,
                vawc.Nature,
                vawc.Status,
                vawc.Narrative,
                vawc.CreatedAt
                );

            return response;
        }

        
    }
}
