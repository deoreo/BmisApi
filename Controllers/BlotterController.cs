using BmisApi.Logging;
using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class BlotterController : ControllerBase
    {
        private readonly ICrudService
            <Blotter, GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest> _service;
        public BlotterController(ICrudService
            <Blotter, GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest> service)
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
                return BadRequest("Failed to register incident.");
            }

            // TODO: Change to CREATED instead of OK
            //return CreatedAtAction(
            //    nameof(GetResidentByIdAsync),
            //    new { id = response.ResidentId },
            //    response);
            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteBlotterAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
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
    }
}
