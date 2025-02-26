using BmisApi.Models;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Repositories;
using Microsoft.EntityFrameworkCore;
using static BmisApi.Services.PictureService;

namespace BmisApi.Services.ResidentService.ResidentService
{
    public class ResidentService : IResidentService
    {
        private readonly ICrudRepository<Resident> _repository;
        private readonly string _uploadPath;
        private readonly PictureService _pictureService;

        public ResidentService(ICrudRepository<Resident> repository, IConfiguration config, PictureService pictureService)
        {
            _repository = repository;
            _uploadPath = config["Storage:UploadPath"] ?? "uploads";
            _pictureService = pictureService;
        }

        public async Task<GetResidentResponse?> GetByIdAsync(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null)
            {
                throw new KeyNotFoundException($"Resident with ID {id} not found");
            }

            return SetResponse(resident);
        }

        public async Task<GetResidentResponse> CreateAsync(CreateResidentRequest request)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Today);
            if (request.Birthday >= dateNow)
            {
                throw new Exception("Invalid birthday");
            }

            var resident = new Resident
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Suffix = request.Suffix,
                Sex = request.Sex,
                Birthday = request.Birthday,
                Occupation = request.Occupation,
                RegisteredVoter = request.RegisteredVoter
            };

            resident = await _repository.CreateAsync(resident);

            return SetResponse(resident);
        }

        public async Task DeleteAsync(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null)
                throw new KeyNotFoundException($"Resident with ID {id} not found");

            if (!string.IsNullOrEmpty(resident.PicturePath))
            {
                _pictureService.DeletePictureFile(resident.PicturePath);
                resident.PicturePath = null;
                await _repository.UpdateAsync(resident);
            }

            await _repository.DeleteAsync(id);
        }

        public async Task<GetResidentResponse?> UpdateAsync(UpdateResidentRequest request, int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null)
            {
                throw new KeyNotFoundException($"Resident with ID {id} not found");
            }

            resident.FirstName = request.FirstName;
            resident.MiddleName = request.MiddleName;
            resident.LastName = request.LastName;
            resident.Suffix = request.Suffix;
            resident.Sex = request.Sex;
            resident.Birthday = request.Birthday;
            resident.Occupation = request.Occupation;
            resident.RegisteredVoter = request.RegisteredVoter;
            resident.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(resident);

            return SetResponse(resident);
        }

        public async Task<GetAllResidentResponse> GetAllAsync()
        {
            var residents = await _repository.GetAllAsync();

            var residentResponse = residents.Select(SetResponse).ToList();

            return new GetAllResidentResponse(residentResponse);
        }

        public async Task<GetAllResidentResponse> Search(string name)
        {
            var result = await _repository.Search(name).ToListAsync();
            var residentResponse = new List<GetResidentResponse>();

            foreach (var resident in result)
            {
                residentResponse.Add(SetResponse(resident));
            }

            return new GetAllResidentResponse(residentResponse);
        }

        public GetResidentResponse SetResponse(Resident resident)
        {
            var response = new GetResidentResponse
                (
                resident.Id,
                resident.FirstName,
                resident.MiddleName,
                resident.LastName,
                resident.Suffix,
                resident.FullName,
                resident.GetAge(resident.Birthday),
                resident.Sex.ToString(),
                resident.Birthday,
                resident.Occupation,
                resident.RegisteredVoter,
                resident.HouseholdId,
                resident.Household?.Address,
                resident.PicturePath,
                resident.CreatedAt
                );

            return response;
        }

        public async Task<string?> UpdatePictureAsync(int id, IFormFile picture)
        {
            try
            {
                _pictureService.ValidateFile(picture);

                var resident = await _repository.GetByIdAsync(id);
                if (resident == null)
                {
                    throw new KeyNotFoundException($"Resident with ID {id} not found");
                }

                if (!string.IsNullOrEmpty(resident.PicturePath))
                {
                    _pictureService.DeletePictureFile(resident.PicturePath);
                }

                var relativePath = _pictureService.CreatePicturePath("residents");
                var picturePath = await _pictureService.SavePictureFileAsync(picture, relativePath);

                resident.PicturePath = picturePath;
                await _repository.UpdateAsync(resident);

                return picturePath;
            }
            catch (Exception ex) when (ex is not FileValidationException && ex is not KeyNotFoundException)
            {
                throw new Exception("An error occurred while updating the file", ex);
            }

        }

        public async Task DeletePictureAsync(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null)
                throw new KeyNotFoundException($"Resident with ID {id} not found");

            if (!string.IsNullOrEmpty(resident.PicturePath))
            {
                _pictureService.DeletePictureFile(resident.PicturePath);
                resident.PicturePath = null;
                await _repository.UpdateAsync(resident);
            }
        }
    }
}
