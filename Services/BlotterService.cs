using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Repositories;

namespace BmisApi.Services
{
    public class BlotterService : ICrudService<Blotter, GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest>
    {
        private readonly ICrudRepository<Blotter> _blotterRepository;
        private readonly ICrudRepository<Resident> _residentRepository;

        public BlotterService(ICrudRepository<Blotter> blotterRepository, ICrudRepository<Resident> residentRepository)
        {
            _blotterRepository = blotterRepository;
            _residentRepository = residentRepository;
        }

        public async Task<GetBlotterResponse?> GetByIdAsync(int id)
        {
            var blotter = await _blotterRepository.GetByIdAsync(id);
            if (blotter == null)
            {
                return null;
            }

            return SetResponse(blotter);
        }

        public async Task<GetBlotterResponse> CreateAsync(CreateBlotterRequest request)
        {
            var complainant = await _residentRepository.GetByIdAsync(request.ComplainantId);
            if (complainant == null)
            {
                throw new Exception($"Provided complainant resident with id {request.ComplainantId} not found");
            }

            var defendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (defendant == null)
            {
                throw new Exception($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var blotter = new Blotter
            {
                Date = request.Date,
                ComplainantId = request.ComplainantId,
                Complainant = complainant,
                DefendantId = request.DefendantId,
                Defendant = defendant,
                Nature = request.Nature,
                Status = request.Status,
                Narrative = request.Narrative
            };

            blotter = await _blotterRepository.CreateAsync(blotter);

            return SetResponse(blotter);
        }

        public async Task DeleteAsync(int id)
        {
            await _blotterRepository.DeleteAsync(id);
        }

        public async Task<GetBlotterResponse?> UpdateAsync(UpdateBlotterRequest request, int id)
        {
            var newComplainant = await _residentRepository.GetByIdAsync(request.ComplainantId);
            if (newComplainant == null)
            {
                throw new Exception($"Provided complainant resident with id {request.ComplainantId} not found");
            }

            var newDefendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (newDefendant == null)
            {
                throw new Exception($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var blotter = await _blotterRepository.GetByIdAsync(id);
            if (blotter == null)
            {
                return null;
            }

            blotter.Date = request.Date;
            blotter.ComplainantId = request.ComplainantId;
            blotter.Complainant = newComplainant;
            blotter.DefendantId = request.DefendantId;
            blotter.Defendant = newDefendant;
            blotter.Nature = request.Nature;
            blotter.Status = request.Status;
            blotter.Narrative = request.Narrative;
            blotter.LastUpdatedAt = DateTime.UtcNow;

            await _blotterRepository.UpdateAsync(blotter);

            return SetResponse(blotter);
        }

        public async Task<GetAllBlotterResponse> GetAllAsync()
        {
            var blotters = await _blotterRepository.GetAllAsync();

            var responses = blotters.Select(SetResponse).ToList();

            return new GetAllBlotterResponse(responses);
        }

        public Task<GetAllBlotterResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetBlotterResponse SetResponse(Blotter blotter)
        {
            var response = new GetBlotterResponse
                (
                blotter.Id,
                blotter.Date,
                blotter.Complainant.FullName,
                blotter.Defendant.FullName,
                blotter.Nature,
                blotter.Status,
                blotter.Narrative,
                blotter.CreatedAt
                );

            return response;
        }


    }
}
