using BmisApi.Logging;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Justice;
using BmisApi.Models.DTOs.Narrative;
using BmisApi.Services.BlotterService;
using BmisApi.Services.JusticeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class JusticeController : ControllerBase
    {
        private readonly IJusticeService _service;
        public JusticeController(IJusticeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<ActionResult<GetJusticeResponse>> GetJusticeByIdAsync(int id)
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
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<ActionResult<GetJusticeResponse>> CreateJusticeAsync(CreateJusticeRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register justice.");
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteJusticeAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<GetJusticeResponse>> UpdateJusticeAsync(int id, UpdateJusticeRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if (response is null)
            {
                return BadRequest("Failed to update justice.");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllJusticeResponse>> GetAllJusticeAsync()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get justices");
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("get-narratives")]
        [NoAuditLog]
        [Authorize(Policy = "RequireSecretaryRole")]
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
        [Authorize(Policy = "RequireSecretaryRole")]
        public IActionResult ExportAsync()
        {
            return Ok();
        }
    }
}
