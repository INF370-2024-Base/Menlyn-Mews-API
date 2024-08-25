using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Menlyn_Mews_API.Filters
{
    public class AuditLogFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next(); // This executes the action

            var auditLog = new Audit_Log
            {
                User_Name = context.HttpContext.User?.Identity?.Name ?? "Anonymous",
                Action = context.HttpContext.Request.Method,
                Controller_Name = context.ActionDescriptor.RouteValues["controller"],
                Action_Name = context.ActionDescriptor.RouteValues["action"],
                Timestamp = DateTime.UtcNow,
                Details = context.HttpContext.Request.QueryString.HasValue ? context.HttpContext.Request.QueryString.Value : context.HttpContext.Request.Path
            };

            var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();
            dbContext.AuditLogs.Add(auditLog);
            await dbContext.SaveChangesAsync();
        }
    }
}
