﻿using BmisApi.Models.DTOs.Blotter;
using BmisApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BmisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlotterController : ControllerBase
    {
        private readonly ICrudService
            <GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest> _service;
        public BlotterController(ICrudService
            <GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest> service)
        {
            _service = service;
        }
    }
}