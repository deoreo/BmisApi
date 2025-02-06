using BmisApi.Logging;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Incident;
using BmisApi.Services.IncidentService;
using BmisApi.Services.VawcService;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class VawcController : ControllerBase
    {
        private readonly IVawcService _service;
        public VawcController(IVawcService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<GetVawcResponse>> GetVawcByIdAsync(int id)
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
        public async Task<ActionResult<GetVawcResponse>> CreateVawcAsync(CreateVawcRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register VAWC.");
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
        public async Task<ActionResult> DeleteVawcAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<ActionResult<GetVawcResponse>> UpdateIncidentAsync(int id, UpdateVawcRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if (response is null)
            {
                return BadRequest("Failed to update VAWC");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<GetAllVawcResponse>> GetAllVawcResponse()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get VAWC");
            }

            return Ok(response);
        }
    }
}
