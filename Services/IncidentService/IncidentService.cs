using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Incident;
using BmisApi.Repositories;
using NuGet.Protocol.Core.Types;
using static BmisApi.Services.PictureService;

namespace BmisApi.Services.IncidentService
{
    public class IncidentService : IIncidentService
    {
        private readonly ICrudRepository<Incident> _incidentRepository;
        private readonly ICrudRepository<Resident> _residentRepository;
        private readonly PictureService _pictureService;

        public IncidentService(ICrudRepository<Incident> incidentRepository, ICrudRepository<Resident> residentRepository, PictureService pictureService)
        {
            _incidentRepository = incidentRepository;
            _residentRepository = residentRepository;
            _pictureService = pictureService;
        }

        public async Task<GetIncidentResponse?> GetByIdAsync(int id)
        {
            var incident = await _incidentRepository.GetByIdAsync(id);
            if (incident == null)
            {
                return null;
            }

            return SetResponse(incident);
        }
        public async Task<GetIncidentResponse> CreateAsync(CreateIncidentRequest request)
        {
            var complainant = await _residentRepository.GetByIdAsync(request.ComplainantId);
            if (complainant == null)
            {
                throw new Exception($"Provided complainant resident with id {request.ComplainantId} not found");
            }

            var incident = new Incident
            {
                Date = request.Date,
                CaseId = request.CaseId,
                ComplainantId = request.ComplainantId,
                Complainant = complainant,
                Nature = request.Nature,
                Narrative = request.Narrative
            };

            incident = await _incidentRepository.CreateAsync(incident);

            return SetResponse(incident);
        }

        public async Task DeleteAsync(int id)
        {
            await _incidentRepository.DeleteAsync(id);
        }

        public async Task<GetIncidentResponse?> UpdateAsync(UpdateIncidentRequest request, int id)
        {
            var newComplainant = await _residentRepository.GetByIdAsync(request.ComplainantId);
            if (newComplainant == null)
            {
                throw new Exception($"Provided complainant resident with id {request.ComplainantId} not found");
            }

            var incident = await _incidentRepository.GetByIdAsync(id);
            if (incident == null)
            {
                return null;
            }

            incident.Date = request.Date;
            incident.CaseId = request.CaseId;
            incident.ComplainantId = request.ComplainantId;
            incident.Complainant = newComplainant;
            incident.Nature = request.Nature;
            incident.Narrative = request.Narrative;
            incident.LastUpdatedAt = DateTime.UtcNow;

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
                incident.Complainant.FullName,
                incident.Nature,
                incident.Narrative,
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

    }
}
