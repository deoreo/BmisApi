using BmisApi.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BmisApi.Logging
{
    public class AuditLogAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<NoAuditLogAttribute>() != null )
            {
                await next();
                return;
            }

            var resultContext = await next();

            try
            {
                var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

                var user = context.HttpContext.User;

                string username = user.FindFirstValue(ClaimTypes.Name) ?? "Anonymous";

                string requestPath = context.HttpContext.Request.Path;
                string method = context.HttpContext.Request.Method;

                string action = $"Path: {requestPath} || Method: {method}";

                int statusCode = resultContext.HttpContext.Response.StatusCode;

                var logEntry = new AuditLogModel
                {
                    Username = username,
                    Action = $"{action}",
                    Timestamp = DateTime.UtcNow,
                    StatusCode = statusCode
                };

                dbContext.AuditLogs.Add(logEntry);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create audit log: {ex}");
            }
        }
    }
}
