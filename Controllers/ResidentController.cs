using BmisApi.Logging;
using BmisApi.Models;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Services;
using BmisApi.Services.ResidentService.ResidentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static BmisApi.Services.PictureService;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        private readonly IResidentService _service;
        public ResidentController(IResidentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<ActionResult<GetResidentResponse>> GetResidentByIdAsync(int id)
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
        public async Task<ActionResult<GetResidentResponse>> CreateResidentAsync(CreateResidentRequest request)
        { 
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register resident");
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteResidentAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Policy ="RequireAdminRole")]
        [HttpPut]
        [Route("edit/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<GetResidentResponse>> UpdateResidentAsync(int id, UpdateResidentRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if ( response == null)
            {
                return BadRequest("Failed to update resident");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllResidentResponse>> GetAllResidentAsync()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get residents");
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<GetAllResidentResponse>> Search(string name)
        {
            var result = await _service.Search(name);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
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
