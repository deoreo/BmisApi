using BmisApi.Models;
using BmisApi.Models.DTOs.BrgyProject;
using BmisApi.Models.DTOs.Narrative;
using BmisApi.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BmisApi.Services.NarrativeService
{
    public class NarrativeService : INarrativeService
    {
        private readonly ICrudRepository<Narrative> _repository;

        public NarrativeService(ICrudRepository<Narrative> repository)
        {
            _repository = repository;
        }

        public async Task<GetNarrativeResponse> CreateAsync(CreateNarrativeRequest request)
        {
            var narrative = new Narrative()
            {
                ReportId = request.ReportId,
                Status = request.Status,
                NarrativeReport = request.NarrativeReport,
                Date = request.Date
            };

            await _repository.CreateAsync(narrative);

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
            var response = new GetNarrativeResponse
                (
                entity.Id,
                entity.ReportId,
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

            narrative.ReportId = request.ReportId;
            narrative.Status = request.Status;
            narrative.NarrativeReport = request.NarrativeReport;
            narrative.Date = request.Date;
            narrative.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(narrative);

            return SetResponse(narrative);
        }
    }
}
