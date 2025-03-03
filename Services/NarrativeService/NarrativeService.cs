using BmisApi.Models;
using BmisApi.Models.DTOs.Narrative;
using BmisApi.Repositories;

namespace BmisApi.Services.NarrativeService
{
    public class NarrativeService : INarrativeService
    {
        private readonly ICrudRepository<Narrative> _repository;
        private readonly ICrudRepository<Blotter> _blotterRepository;
        private readonly ICrudRepository<Vawc> _vawcRepository;
        private readonly ICrudRepository<Justice> _justiceRepository;

        public NarrativeService(ICrudRepository<Narrative> repository,
            ICrudRepository<Blotter> blotterRepository,
            ICrudRepository<Vawc> vawcRepository,
            ICrudRepository<Justice> justiceRepository)
        {
            _repository = repository;
            _blotterRepository = blotterRepository;
            _vawcRepository = vawcRepository;
            _justiceRepository = justiceRepository;
        }

        public async Task<GetNarrativeResponse> CreateAsync(CreateNarrativeRequest request)
        {
            ValidateForeignKeys(request.BlotterId, request.VawcId, request.JusticeId);

            var dateNow = DateOnly.FromDateTime(DateTime.Today);
            if (request.Date > dateNow)
            {
                throw new Exception("Invalid date");
            }

            var narrative = new Narrative()
            {
                BlotterId = request.BlotterId,
                VawcId = request.VawcId,
                JusticeId = request.JusticeId,
                Status = request.Status,
                NarrativeReport = request.NarrativeReport,
                Date = request.Date,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateAsync(narrative);

            await AssignNewValuesToReport(narrative, request);

            return SetResponse(narrative);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<GetAllNarrativeResponse> GetAllAsync()
        {
            var narratives = await _repository.GetAllAsync();

            var responses = narratives.Select(SetResponse).ToList();

            return new GetAllNarrativeResponse(responses);
        }

        public async Task<GetNarrativeResponse?> GetByIdAsync(int id)
        {
            var narrative = await _repository.GetByIdAsync(id);
            if (narrative == null)
            {
                throw new KeyNotFoundException($"Narrative with ID {id} not found");
            }

            return SetResponse(narrative);
        }

        public Task<GetAllNarrativeResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetNarrativeResponse SetResponse(Narrative entity)
        {
            int reportId;
            if (entity.BlotterId != null)
            {
                reportId = entity.BlotterId.Value;
            }
            else if (entity.VawcId != null)
            {
                reportId = entity.VawcId.Value;
            }
            else if (entity.JusticeId != null)
            {
                reportId = entity.JusticeId.Value;
            }
            else
            {
                throw new Exception("Invalid narrative report");
            }

            var response = new GetNarrativeResponse
                (
                entity.Id,
                entity.CaseId,
                reportId,
                entity.Status,
                entity.NarrativeReport,
                entity.Date,
                entity.CreatedAt
                );

            return response;
        }

        public async Task<GetNarrativeResponse?> UpdateAsync(UpdateNarrativeRequest request, int id)
        {
            var narrative = await _repository.GetByIdAsync(id);
            if (narrative == null)
            {
                throw new KeyNotFoundException($"Narrative with ID {id} not found");
            }

            narrative.Status = request.Status;
            narrative.NarrativeReport = request.NarrativeReport;
            narrative.Date = request.Date;
            narrative.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(narrative);
            return SetResponse(narrative);
        }

        private void ValidateForeignKeys(int? blotterId, int? vawcId, int? justiceId)
        {
            int count = (blotterId.HasValue ? 1 : 0) +
                        (vawcId.HasValue ? 1 : 0) +
                        (justiceId.HasValue ? 1 : 0);

            if (count != 1)
            {
                throw new ArgumentException("A narrative must be associated with exactly one of Blotter, Vawc, Justice.");
            }
        }

        private async Task AssignNewValuesToReport(Narrative narrative, CreateNarrativeRequest request)
        {
            if (request.BlotterId != null)
            {
                var blotter = await _blotterRepository.GetByIdAsync(request.BlotterId.Value);
                if (blotter == null)
                {
                    throw new KeyNotFoundException($"Blotter with ID {request.BlotterId} not found");
                }
                blotter.Status = request.Status;
                blotter.Date = request.Date;

                await _blotterRepository.UpdateAsync(blotter);

                narrative.CaseId = blotter.CaseId;

                await _repository.UpdateAsync(narrative);

            }
            else if (request.VawcId != null)
            {
                var vawc = await _vawcRepository.GetByIdAsync(request.VawcId.Value);
                if (vawc == null)
                {
                    throw new KeyNotFoundException($"VAWC with ID {request.VawcId} not found");
                }

                vawc.Status = request.Status;
                vawc.Date = request.Date;

                await _vawcRepository.UpdateAsync(vawc);

                narrative.CaseId = vawc.CaseId;

                await _repository.UpdateAsync(narrative);
            }
            else if (request.JusticeId != null)
            {
                var justice = await _justiceRepository.GetByIdAsync(request.JusticeId.Value);
                if (justice == null)
                {
                    throw new KeyNotFoundException($"Justice with ID {request.JusticeId} not found");
                }

                justice.Status = request.Status;
                justice.Date = request.Date;

                await _justiceRepository.UpdateAsync(justice);

                narrative.CaseId = justice.CaseId;

                await _repository.UpdateAsync(narrative);
            }
        }
    }
}
