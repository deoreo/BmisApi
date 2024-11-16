using BmisApi.Models;
using BmisApi.Models.DTOs;
using BmisApi.Repositories;
using BmisApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class ResidentController : ControllerBase
    {
        private readonly IResidentRepository _repository;
        private readonly ResidentService _service;
        public ResidentController(IResidentRepository repository, ResidentService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<GetResidentResponse>> GetResidentByIdAsync(int id)
        {
            var resident = await _repository.GetResidentByIdAsync(id);

            if (resident == null) 
                return NotFound();

            var response = _service.SetResidentResponse(resident);

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<GetResidentResponse>> CreateResidentAsync(CreateResidentRequest request)
        {
            var resident = new Resident
                (
                    request.FullName,
                    request.Sex,
                    request.Birthday,
                    request.Occupation,
                    request.RegisteredVoter
                );  

            if (resident == null)
            {
                return BadRequest("Failed to create resident.");
            }

            resident = await _repository.CreateResidentAsync(resident);

            var response = _service.SetResidentResponse(resident);

            //return CreatedAtAction(
            //    nameof(GetResidentByIdAsync),
            //    new { id = response.ResidentId },
            //    response);
            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteResidentAsync(int id)
        {
            await _repository.DeleteResidentAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<ActionResult<GetResidentResponse>> UpdateResidentAsync(int id, UpdateResidentRequest request)
        {
            var resident = await _repository.GetResidentByIdAsync(id);

            if (resident is null)
            {
                return BadRequest("Failed to update resident");
            }

            resident.FullName = request.FullName;
            resident.Sex = request.Sex;
            resident.Birthday = request.Birthday;
            resident.Occupation = request.Occupation;
            resident.RegisteredVoter = request.RegisteredVoter;
            resident.HouseholdId = request.HouseholdId;
            resident.CreatedAt = DateTime.UtcNow;

            // TODO: Write a check for when householdId refers to non-existing household
            await _repository.UpdateResidentAsync(resident);

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<GetAllResidentResponse>> GetAllResidentAsync()
        {
            var residents = await _repository.GetAllResidentsAsync()
                .ToListAsync();
            var residentResponse = new List<GetResidentResponse>();

            foreach ( var resident in residents )
            {
                residentResponse.Add(_service.SetResidentResponse(resident));
            }

            var allResidentResponse = new GetAllResidentResponse(residentResponse);

            return Ok(allResidentResponse);
        }

        [HttpGet]
        [Route("{search}")]
        public ActionResult<GetAllResidentResponse> Search(string name, Sex? sex)
        {
            try
            {
                var result = _repository.Search(name, sex);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the databe.");
            }
        }

    }
}
