﻿using BmisApi.Models;
using BmisApi.Models.DTOs.Incident;
using BmisApi.Repositories;
using static BmisApi.Services.PictureService;

namespace BmisApi.Services.IncidentService
{
    public class IncidentService : IIncidentService
    {
        private readonly ICrudRepository<Incident> _incidentRepository;
        private readonly PictureService _pictureService;

        public IncidentService(ICrudRepository<Incident> incidentRepository, 
            PictureService pictureService)
        {
            _incidentRepository = incidentRepository;
            _pictureService = pictureService;
        }

        public async Task<GetIncidentResponse?> GetByIdAsync(int id)
        {
            var incident = await _incidentRepository.GetByIdAsync(id);
            if (incident == null)
            {
                throw new KeyNotFoundException($"Incident with ID {id} not found");
            }

            return SetResponse(incident);
        }
        public async Task<GetIncidentResponse> CreateAsync(CreateIncidentRequest request)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Today);
            if (request.Date > dateNow)
            {
                throw new Exception("Invalid date");
            }

            var incident = new Incident
            {
                Date = request.Date,
                CaseId = await GenerateCaseIdAsync(),
                Complainants = request.Complainants,
                Nature = request.Nature,
                NarrativeReport = request.Narrative,
            };
            incident = await _incidentRepository.CreateAsync(incident);

            return SetResponse(incident);
        }

        public async Task DeleteAsync(int id)
        {
            var incident = await _incidentRepository.GetByIdAsync(id);
            if (incident == null)
                throw new KeyNotFoundException($"Incident with ID {id} not found");

            if (!string.IsNullOrEmpty(incident.PicturePath))
            {
                _pictureService.DeletePictureFile(incident.PicturePath);
                incident.PicturePath = null;
                await _incidentRepository.UpdateAsync(incident);
            }

            await _incidentRepository.DeleteAsync(id);
        }

        public async Task<GetIncidentResponse?> UpdateAsync(UpdateIncidentRequest request, int id)
        {
            var incident = await _incidentRepository.GetByIdAsync(id);
            if (incident == null)
            {
                throw new KeyNotFoundException($"Incident with ID {id} not found");
            }

            incident.Complainants = request.Complainants;
            incident.Nature = request.Nature;
            incident.LastUpdatedAt = DateTime.UtcNow;
            incident.NarrativeReport = request.Narrative;

            await _incidentRepository.UpdateAsync(incident);

            return SetResponse(incident);
        }

        public async Task<GetAllIncidentResponse> GetAllAsync()
        {
            var incident = await _incidentRepository.GetAllAsync();

            var responses = incident.Select(SetResponse).ToList();

            return new GetAllIncidentResponse(responses);
        }

        public Task<GetAllIncidentResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetIncidentResponse SetResponse(Incident incident)
        {
            var response = new GetIncidentResponse
                (
                incident.Id,
                incident.CaseId,
                incident.Date,
                incident.Complainants,
                incident.Nature,
                incident.NarrativeReport,
                incident.PicturePath,
                incident.CreatedAt
                );

            return response;
        }

        public async Task<string?> UpdatePictureAsync(int id, IFormFile picture)
        {
            try
            {
                _pictureService.ValidateFile(picture);

                var incident = await _incidentRepository.GetByIdAsync(id);
                if (incident == null)
                {
                    throw new KeyNotFoundException($"Incident with ID {id} not found");
                }

                if (!string.IsNullOrEmpty(incident.PicturePath))
                {
                    _pictureService.DeletePictureFile(incident.PicturePath);
                }

                var relativePath = _pictureService.CreatePicturePath("incidents");
                var picturePath = await _pictureService.SavePictureFileAsync(picture, relativePath);

                incident.PicturePath = picturePath;
                await _incidentRepository.UpdateAsync(incident);

                return picturePath;
            }
            catch (Exception ex) when (ex is not FileValidationException && ex is not KeyNotFoundException)
            {
                throw new Exception("An error occurred while updating the file", ex);
            }
        }

        public async Task DeletePictureAsync(int id)
        {
            var incident = await _incidentRepository.GetByIdAsync(id);
            if (incident == null)
                throw new KeyNotFoundException($"Incident with ID {id} not found");

            if (!string.IsNullOrEmpty(incident.PicturePath))
            {
                _pictureService.DeletePictureFile(incident.PicturePath);
                incident.PicturePath = null;
                await _incidentRepository.UpdateAsync(incident);
            }
        }

        private async Task<string> GenerateCaseIdAsync()
        {
            int year = DateTime.UtcNow.Year % 100; // Get last two digits of the year
            int nextNumber = 1;

            var allBlotters = await _incidentRepository.GetAllAsync();

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
    }
}
