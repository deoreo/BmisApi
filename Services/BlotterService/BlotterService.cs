using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models;
using BmisApi.Repositories;
using BmisApi.Models.DTOs.Narrative;
using System.Globalization;

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
                CaseId = await GenerateCaseIdAsync(),
                Complainant = ToTitleCase(request.Complainant).Trim(),
                ContactInfo = request.ContactInfo?.Trim(),
                DefendantId = request.DefendantId,
                Defendant = defendant,
                Nature = ToTitleCase(request.Nature),
                Status = request.Status,
                NarrativeReports = narratives
            };
            await _blotterRepository.CreateAsync(blotter);

            var narrative = new Narrative
            {
                CaseId = blotter.CaseId,
                BlotterId = blotter.Id,
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
            var newDefendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (newDefendant == null)
            {
                throw new KeyNotFoundException($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var blotter = await _blotterRepository.GetByIdAsync(id);
            if (blotter == null)
            {
                throw new KeyNotFoundException($"Blotter with ID {id} not found");
            }

            blotter.Complainant = ToTitleCase(request.Complainant).Trim();
            blotter.ContactInfo = request.ContactInfo?.Trim();
            blotter.DefendantId = request.DefendantId;
            blotter.Defendant = newDefendant;
            blotter.Nature = ToTitleCase(request.Nature);
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
                    narratives.Id,
                    narratives.CaseId,
                    narratives.BlotterId,
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
                blotter.Complainant,
                blotter.ContactInfo ?? "N/A",
                blotter.Defendant.FullName,
                blotter.Nature,
                blotter.Status,
                blotter.CreatedAt
                );

            return response;
        }

        private async Task<string> GenerateCaseIdAsync()
        {
            int year = DateTime.UtcNow.Year % 100; // Get last two digits of the year
            int nextNumber = 1;

            var allBlotters = await _blotterRepository.GetAllAsync();

            var latestCase = allBlotters
                .Where(b => b.CaseId.EndsWith($"-{year}"))
                .OrderByDescending(b => b.CreatedAt) 
                .FirstOrDefault();

            if (latestCase != null)
            {
                string[] parts = latestCase.CaseId.Split('-');
                if (int.TryParse(parts[0], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{nextNumber:D2}-{year}"; // Format as 2-digit number with year (e.g., "01-25")
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
