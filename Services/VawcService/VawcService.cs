using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Narrative;
using BmisApi.Repositories;
using System.Globalization;

namespace BmisApi.Services.VawcService
{
    public class VawcService : IVawcService
    {
        private readonly ICrudRepository<Vawc> _vawcRepository;
        private readonly ICrudRepository<Resident> _residentRepository;
        private readonly ICrudRepository<Narrative> _narrativeRepository;

        public VawcService(ICrudRepository<Vawc> vawcRepository, ICrudRepository<Resident> residentRepository, ICrudRepository<Narrative> narrativeRepository)
        {
            _vawcRepository = vawcRepository;
            _residentRepository = residentRepository;
            _narrativeRepository = narrativeRepository;
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
            var dateNow = DateOnly.FromDateTime(DateTime.Today);
            if (request.Date > dateNow)
            {
                throw new Exception("Invalid date");
            }

            var defendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (defendant == null)
            {
                throw new KeyNotFoundException($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var narratives = new List<Narrative> { };

            var vawc = new Vawc
            {
                Date = request.Date,
                CaseId = await GenerateCaseIdAsync(),
                Complainant = ToTitleCase(request.Complainant).Trim(),
                ContactInfo = request.ContactInfo?.Trim(),
                DefendantId = request.DefendantId,
                Defendant = defendant,
                Nature = ToTitleCase(request.Nature).Trim(),
                Status = request.Status,
                NarrativeReports = narratives
            };
            vawc = await _vawcRepository.CreateAsync(vawc);

            var narrative = new Narrative
            {
                CaseId = vawc.CaseId,
                VawcId = vawc.Id,
                Status = request.Status,
                NarrativeReport = request.Narrative,
                Date = request.Date,
            };
            await _narrativeRepository.CreateAsync(narrative);

            return SetResponse(vawc);
        }

        public async Task DeleteAsync(int id)
        {
            await _vawcRepository.DeleteAsync(id);
        }

        public async Task<GetVawcResponse?> UpdateAsync(UpdateVawcRequest request, int id)
        {
            var newDefendant = await _residentRepository.GetByIdAsync(request.DefendantId);
            if (newDefendant == null)
            {
                throw new KeyNotFoundException($"Provided defendant resident with id {request.DefendantId} not found.");
            }

            var vawc = await _vawcRepository.GetByIdAsync(id);
            if (vawc == null)
            {
                throw new KeyNotFoundException($"VAWC with ID {id} not found");
            }

            vawc.Complainant = ToTitleCase(request.Complainant).Trim();
            vawc.ContactInfo = request.ContactInfo?.Trim();
            vawc.Complainant = request.Complainant;
            vawc.DefendantId = request.DefendantId;
            vawc.Defendant = newDefendant;
            vawc.Nature = ToTitleCase(request.Nature).Trim();
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
                vawc.Complainant,
                vawc.ContactInfo,
                vawc.Defendant.FullName,
                vawc.Nature,
                vawc.Status,
                vawc.CreatedAt
                );

            return response;
        }

        private async Task<string> GenerateCaseIdAsync()
        {
            int year = DateTime.UtcNow.Year % 100; // Get last two digits of the year
            int nextNumber = 1;

            var allBlotters = await _vawcRepository.GetAllAsync();

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

        public async Task<GetAllNarrativeResponse> GetNarrativesAsync(int id)
        {
            var vawc = await _vawcRepository.GetByIdAsync(id);
            if (vawc == null)
            {
                throw new KeyNotFoundException($"Provided vawc with id {id} not found");
            }

            var narratives = vawc.NarrativeReports
                .OrderBy(n => n.CreatedAt)
                .Select(narratives => new GetNarrativeResponse
                (
                    narratives.Id,
                    narratives.CaseId,
                    narratives.VawcId,
                    narratives.Status,
                    narratives.NarrativeReport,
                    narratives.Date,
                    narratives.CreatedAt
                ))
                .ToList();

            return new GetAllNarrativeResponse(narratives);
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
