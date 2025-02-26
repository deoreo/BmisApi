using BmisApi.Logging;
using BmisApi.Services.ResidentService.ResidentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireClerkRole")]
    public class CertificateController : ControllerBase
    {

        [HttpPost]
        [Route("create-indigency")]
        public IActionResult CreateIndigencyAsync()
        {
            return Ok();
        }

        [HttpPost]
        [Route("create-residency")]
        public IActionResult CreateResidencyAsync()
        {
            return Ok();
        }

        [HttpPost]
        [Route("create-clearance")]
        public IActionResult CreateClearanceAsync()
        {
            return Ok();
        }
    }
}
