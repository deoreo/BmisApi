using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models;
using BmisApi.Repositories;
using BmisApi.Models.DTOs.Narrative;

namespace BmisApi.Services.BlotterService
{
    public class BlotterService : IBlotterService
    {
        private readonly ICrudRepository<Blotter> _blotterRepository;
        private readonly ICrudRepository<Resident> _residentRepository;
        private readonly ICrudRepository<Narrative> _narrativeRepository;

        public BlotterService(ICrudRepository<Blotter> blotterRepository,

            ICrudRepository<Resident> residentRepository,
            ICrudRepository<Narrative> narrativeRepository)
        {
            _blotterRepository = blotterRepository;
            _residentRepository = residentRepository;
            _narrativeRepository = narrativeRepository;
        }

        public async Task<GetBlotterResponse?> GetByIdAsync(int id)
        {
            var blotter = await _blotterRepository.GetByIdAsync(id);
            if (blotter == null)
            {
                throw new KeyNotFoundException($"Blotter with ID {id} not found");
            }

            return SetResponse(blotter);
        }

        public async Task<GetBlotterResponse> CreateAsync(CreateBlotterRequest request)
        {
            var complainant = await _residentRepository.GetByIdAsync(request.ComplainantId);
            if (complainant == null)
            {
                throw new KeyNotFoundException($"Provided complainant resident with id {request.ComplainantId} not found");
            }

            var defendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (defendant == null)
            {
                throw new KeyNotFoundException($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var dateNow = DateOnly.FromDateTime(DateTime.Today);
            if (request.Date > dateNow)
            {
                throw new Exception("Invalid date");
            }

            

            var narratives = new List<Narrative> {  };

            var blotter = new Blotter
            {
                Date = request.Date,
                CaseId = request.CaseId,
                ComplainantId = request.ComplainantId,
                Complainant = complainant,
                DefendantId = request.DefendantId,
                Defendant = defendant,
                Nature = request.Nature,
                Status = request.Status,
                NarrativeReports = narratives
            };
            await _blotterRepository.CreateAsync(blotter);

            var narrative = new Narrative
            {
                ReportId = blotter.Id,
                Status = request.Status,
                NarrativeReport = request.Narrative,
                Date = request.Date,
            };
            await _narrativeRepository.CreateAsync(narrative);

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

            var blotter = await _blotterRepository.GetByIdAsync(id);
            if (blotter == null)
            {
                throw new KeyNotFoundException($"Blotter with ID {id} not found");
            }

            blotter.Date = request.Date;
            blotter.CaseId = request.CaseId;
            blotter.ComplainantId = request.ComplainantId;
            blotter.Complainant = newComplainant;
            blotter.DefendantId = request.DefendantId;
            blotter.Defendant = newDefendant;
            blotter.Nature = request.Nature;
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

        public async Task<GetAllNarrativeResponse> GetNarrativesAsync(int id)
        {
            var blotter = await _blotterRepository.GetByIdAsync(id);
            if (blotter == null)
            {
                throw new KeyNotFoundException($"Provided blotter with id {id} not found");
            }

            var narratives = blotter.NarrativeReports
                .OrderBy(n => n.CreatedAt)
                .Select(narratives => new GetNarrativeResponse
                (
                    narratives.ReportId,
                    narratives.Status,
                    narratives.NarrativeReport,
                    narratives.Date,
                    narratives.CreatedAt
                ))
                .ToList();

            return new GetAllNarrativeResponse(narratives);
        }

        public GetBlotterResponse SetResponse(Blotter blotter)
        {
            var response = new GetBlotterResponse
                (
                blotter.Id,
                blotter.CaseId,
                blotter.Date,
                blotter.Complainant.FullName,
                blotter.Defendant.FullName,
                blotter.Nature,
                blotter.Status,
                blotter.CreatedAt
                );

            return response;
        }


    }
}
