
using BmisApi.Models.DTOs.BrgyProject;
using BmisApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrgyProjectController : ControllerBase
    {
        private readonly ICrudService
                <GetBrgyProjectResponse, GetAllBrgyProjectResponse, CreateBrgyProjectRequest, UpdateBrgyProjectRequest> _service;
        public BrgyProjectController(ICrudService
            <GetBrgyProjectResponse, GetAllBrgyProjectResponse, CreateBrgyProjectRequest, UpdateBrgyProjectRequest> service)
        {
            _service = service;
        }
    }
}
