using BmisApi.Logging;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.Household;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Services.OfficialService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireSecretaryRole")]
    public class OfficialController : ControllerBase
    {
        private readonly IOfficialService _service;

        public OfficialController(IOfficialService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<GetOfficialResponse>> GetOfficialByIdAsync(int id)
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
        public async Task<IActionResult> CreateOfficialasync([FromBody]CreateOfficialRequest request)
        {
            //var response = await _service.CreateAsync(request);
            //if (response == null)
            //{
            //    return BadRequest("Failed to register official.");
            //}

            try
            {
                var response = await _service.CreateAsync(request);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { messagae = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { messagae = ex.Message });
            }

        }

        [HttpPut]
        [Route("delete/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteOfficialAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<GetOfficialResponse>> UpdateOfficialAsync(int id, UpdateOfficialRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if (response is null)
            {
                return BadRequest("Failed to update official");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllOfficialResponse>> GetAllOfficialAsync()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get officials");
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
