using BmisApi.Models.DTOs.Narrative;
using BmisApi.Models;
using BmisApi.Repositories;
using System.Globalization;
using BmisApi.Models.DTOs.Justice;

namespace BmisApi.Services.JusticeService
{
    public class JusticeService : IJusticeService
    {
        private readonly ICrudRepository<Justice> _justiceRepository;
        private readonly ICrudRepository<Resident> _residentRepository;
        private readonly ICrudRepository<Narrative> _narrativeRepository;

        public JusticeService(ICrudRepository<Justice> justiceRepository,
            ICrudRepository<Resident> residentRepository,
            ICrudRepository<Narrative> narrativeRepository)
        {
            _justiceRepository = justiceRepository;
            _residentRepository = residentRepository;
            _narrativeRepository = narrativeRepository;
        }

        public async Task<GetJusticeResponse?> GetByIdAsync(int id)
        {
            var justice = await _justiceRepository.GetByIdAsync(id);
            if (justice == null)
            {
                throw new KeyNotFoundException($"Justice with ID {id} not found");
            }

            return SetResponse(justice);
        }

        public async Task<GetJusticeResponse> CreateAsync(CreateJusticeRequest request)
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

            var narratives = new List<Narrative> { };

            var justice = new Justice
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
            await _justiceRepository.CreateAsync(justice);

            var narrative = new Narrative
            {
                CaseId = justice.CaseId,
                BlotterId = justice.Id,
                Status = request.Status,
                NarrativeReport = request.Narrative,
                Date = request.Date,
            };
            await _narrativeRepository.CreateAsync(narrative);

            return SetResponse(justice);
        }

        public async Task DeleteAsync(int id)
        {
            await _justiceRepository.DeleteAsync(id);
        }

        public async Task<GetJusticeResponse?> UpdateAsync(UpdateJusticeRequest request, int id)
        {
            var newDefendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (newDefendant == null)
            {
                throw new KeyNotFoundException($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var justice = await _justiceRepository.GetByIdAsync(id);
            if (justice == null)
            {
                throw new KeyNotFoundException($"Justice with ID {id} not found");
            }

            justice.Complainant = ToTitleCase(request.Complainant).Trim();
            justice.ContactInfo = request.ContactInfo?.Trim();
            justice.DefendantId = request.DefendantId;
            justice.Defendant = newDefendant;
            justice.Nature = ToTitleCase(request.Nature);
            justice.LastUpdatedAt = DateTime.UtcNow;

            await _justiceRepository.UpdateAsync(justice);

            return SetResponse(justice);
        }

        public async Task<GetAllJusticeResponse> GetAllAsync()
        {
            var justice = await _justiceRepository.GetAllAsync();

            var responses = justice.Select(SetResponse).ToList();

            return new GetAllJusticeResponse(responses);
        }

        public Task<GetAllJusticeResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<GetAllNarrativeResponse> GetNarrativesAsync(int id)
        {
            var blotter = await _justiceRepository.GetByIdAsync(id);
            if (blotter == null)
            {
                throw new KeyNotFoundException($"Provided justice with id {id} not found");
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

        public GetJusticeResponse SetResponse(Justice justice)
        {
            var response = new GetJusticeResponse
                (
                justice.Id,
                justice.CaseId,
                justice.Date,
                justice.Complainant,
                justice.ContactInfo ?? "N/A",
                justice.Defendant.FullName,
                justice.Nature,
                justice.Status,
                justice.CreatedAt
                );

            return response;
        }

        private async Task<string> GenerateCaseIdAsync()
        {
            int year = DateTime.UtcNow.Year % 100; // Get last two digits of the year
            int nextNumber = 1;

            var allBlotters = await _justiceRepository.GetAllAsync();

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
