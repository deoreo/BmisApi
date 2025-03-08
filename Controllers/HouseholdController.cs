using BmisApi.Logging;
using BmisApi.Models;
using BmisApi.Models.DTOs.Household;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Services;
using BmisApi.Services.HouseholdService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private readonly IHouseholdService _service;
        public HouseholdController(IHouseholdService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<ActionResult<GetHouseholdResponse>> GetHouseholdByIdAsync(int id)
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
        public async Task<ActionResult<GetHouseholdResponse>> CreateHouseholdAsync(CreateHouseholdRequest request)
        {
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register household.");
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("delete/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteHouseholdAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<GetHouseholdResponse>> UpdateHouseholdAsync(int id, UpdateHouseholdRequest request)
        {
            var response = await _service.UpdateAsync(request, id);
            if (response is null)
            {
                return BadRequest("Failed to update household");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("get-all")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllHouseholdResponse>> GetAllHouseholdAsync()
        {
            var response = await _service.GetAllAsync();
            if (response == null)
            {
                return BadRequest("Failed to get households");
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("get-members")]
        [Authorize(Policy = "RequireSecretaryRole")]
        public async Task<ActionResult<GetAllResidentResponse>> GetMembersAsync(int id)
        {
            var response = await _service.GetMembersAsync(id);
            if (response == null)
            {
                return BadRequest("Failed to get members");
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
