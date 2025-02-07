
using BmisApi.Logging;
using BmisApi.Models;
using BmisApi.Models.DTOs.BrgyProject;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class BrgyProjectController : ControllerBase
    {
        private readonly ICrudService
                <BrgyProject, GetBrgyProjectResponse, GetAllBrgyProjectResponse, CreateBrgyProjectRequest, UpdateBrgyProjectRequest> _service;
        public BrgyProjectController(ICrudService
            <BrgyProject, GetBrgyProjectResponse, GetAllBrgyProjectResponse, CreateBrgyProjectRequest, UpdateBrgyProjectRequest> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<GetBrgyProjectResponse>> GetByIdAsync(int id)
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
        public async Task<ActionResult<GetBrgyProjectResponse>> CreateAsync(CreateBrgyProjectRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register project");
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
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<ActionResult<GetBrgyProjectResponse>> UpdateAsync(int id, UpdateBrgyProjectRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if (response == null)
            {
                return BadRequest("Failed to update project");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllBrgyProjectResponse>> GetAllAsync()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get projects");
            }

            return Ok(response);
        }
    }
}
