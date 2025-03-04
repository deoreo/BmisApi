using BmisApi.Logging;
using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Narrative;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Services;
using BmisApi.Services.BlotterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireWomanDeskRole")]
    public class BlotterController : ControllerBase
    {
        private readonly IBlotterService _service;
        public BlotterController(IBlotterService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<GetBlotterResponse>> GetBlotterByIdAsync(int id)
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
        public async Task<ActionResult<GetBlotterResponse>> CreateBlotterAsync(CreateBlotterRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register blotter.");
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        [Authorize(Policy ="RequireAdminRole")]
        public async Task<ActionResult> DeleteBlotterAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<GetBlotterResponse>> UpdateBlotterAsync(int id, UpdateBlotterRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if (response is null)
            {
                return BadRequest("Failed to update blotter");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllBlotterResponse>> GetAllBlotterAsync()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get blotters");
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("get-narratives")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllNarrativeResponse>> GetNarrativeAsync(int id)
        {
            var response = await _service.GetNarrativesAsync(id);
            if (response == null)
            {
                return BadRequest("Failed to get narratives");
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("export")]
        public IActionResult ExportAsync()
        {
            return Ok();
        }
    }
}
