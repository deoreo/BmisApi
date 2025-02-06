using BmisApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BmisApi.Logging
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-logs")]
        public async Task<IActionResult> GetAuditLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? username = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var userRole = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            var currentUser = User.Identity.Name;

            IQueryable<AuditLogModel> query = _context.AuditLogs.OrderByDescending(l => l.Timestamp);

            if (!userRole.Contains("Admin"))
            {
                // Non-admins can only get their own logs
                query = query.Where(l => l.Username == currentUser);
            }
            else if (!string.IsNullOrEmpty(username))
            {
                // Admin can filter by username
                query = query.Where(l => l.Username == username);
            }

            // Filter by date range
            if (startDate.HasValue)
            {
                query = query.Where(l => l.Timestamp >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(l => l.Timestamp <= endDate.Value);
            }

            // Pagination
            var totalRecords = query.CountAsync();
            var logs = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize,
                logs = logs
            });
        }
    }
}
