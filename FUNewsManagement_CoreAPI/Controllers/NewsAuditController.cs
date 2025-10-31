﻿using FUNewsManagement_CoreAPI.DTOs;
using FUNewsManagement_CoreAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsAuditController : ControllerBase
    {
        private readonly INewsAuditService _auditService;

        public NewsAuditController(INewsAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var audit = _auditService.GetAuditInfo();
            return Ok(new APIResponse<IQueryable<NewsAuditDto>>{
                StatusCode = 200,
                Message = "Success",
                Data = audit
            });
        }
    }

}
