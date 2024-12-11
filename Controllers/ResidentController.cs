using BmisApi.Models;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class ResidentController : ControllerBase
    {
        private readonly ICrudService
            <Resident,GetResidentResponse,GetAllResidentResponse,CreateResidentRequest,UpdateResidentRequest> _service;
        private readonly UserManager<IdentityUser> _userManager;
        public ResidentController(ICrudService
            <Resident,GetResidentResponse, GetAllResidentResponse, CreateResidentRequest, UpdateResidentRequest> service, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("get/{id}")]
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
        public async Task<ActionResult<GetResidentResponse>> CreateResidentAsync(CreateResidentRequest request)
        { 
            var response = await _service.CreateAsync(request);
            if (response == null)
            {
                return BadRequest("Failed to register resident");
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
        public async Task<ActionResult> DeleteResidentAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("edit/{id}")]
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
        [Route("ResetPass")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPass(string userEmail, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result.Errors);
        }
    }
}
