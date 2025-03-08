using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Household;
using BmisApi.Models;
using BmisApi.Services;
using Microsoft.AspNetCore.Mvc;
using BmisApi.Services.IncidentService;
using BmisApi.Models.DTOs.Incident;
using BmisApi.Logging;
using static BmisApi.Services.PictureService;
using Microsoft.AspNetCore.Authorization;
using BmisApi.Models.DTOs.Narrative;

namespace BmisApi.Controllers
{
    [AuditLog]
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
        [Authorize(Policy = "RequireSecretaryRole")]
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
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<ActionResult<GetIncidentResponse>> CreateIncidentAsync(CreateIncidentRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register incident.");
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteIncidentAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
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
        [NoAuditLog]
        public async Task<ActionResult<GetAllIncidentResponse>> GetAllIncidentResponse()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get incidents");
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("upload-picture/{id}")]
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<IActionResult> UploadPicture(int id, IFormFile picture)
        {
            try
            {
                if (picture == null || picture.Length == 0)
                    return BadRequest("No picture provided");

                var path = await _service.UpdatePictureAsync(id, picture);
                return Ok(new { path });
            }
            catch (FileValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error uploading picture");
            }
        }

        [HttpDelete]
        [Route("delete-picture/{id}")]
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            try
            {
                await _service.DeletePictureAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error deleting picture");
            }
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
