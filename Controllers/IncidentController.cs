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
    [Authorize(Policy = "RequireSecretaryRole")]
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
