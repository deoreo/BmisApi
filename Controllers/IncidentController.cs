using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Household;
using BmisApi.Models;
using BmisApi.Services;
using Microsoft.AspNetCore.Mvc;
using BmisApi.Services.IncidentService;
using BmisApi.Models.DTOs.Incident;

namespace BmisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _service;
        public IncidentController(IIncidentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<GetIncidentResponse>> GetIncidentByIdAsync(int id)
        {
            var response = await _service.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<GetIncidentResponse>> CreateIncidentAsync(CreateIncidentRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register incident.");
            }

            //TODO: Change to CREATED instead of OK
            //return CreatedAtAction(
            //    nameof(GetIncidentByIdAsync),
            //    new { id = response.Id },
            //    response);
            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteIncidentAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<ActionResult<GetIncidentResponse>> UpdateIncidentAsync(int id, UpdateIncidentRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if (response is null)
            {
                return BadRequest("Failed to update incident");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<GetAllIncidentResponse>> GetAllIncidentResponse()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get incidents");
            }

            return Ok(response);
        }
    }
}
