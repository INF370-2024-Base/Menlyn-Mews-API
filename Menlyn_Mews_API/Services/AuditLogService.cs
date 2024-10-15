using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Menlyn_Mews_API.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string action, string controllerName, string actionName, string details)
        {
            // Get the logged-in user's claims
            var httpContext = _httpContextAccessor.HttpContext;
            var userName = httpContext?.User?.Identity?.IsAuthenticated == true
                            ? httpContext.User.Identity.Name
                            : "Unknown User"; // Default for non-authenticated users

            // Extract Employee name and surname from claims
            var employeeName = httpContext?.User?.FindFirst("EmployeeName")?.Value; // Ensure you set this in your sign-in logic
            var employeeSurname = httpContext?.User?.FindFirst("EmployeeSurname")?.Value; // Ensure you set this in your sign-in logic

            var auditLog = new Audit_Log
            {
                User_Name = $"{employeeName} {employeeSurname}", // Combine name and surname
                Action = action,
                Controller_Name = controllerName,
                Action_Name = actionName,
                Timestamp = DateTime.UtcNow,
                Details = details
            };

            _context.AuditLogs.Add(auditLog);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log to a file, send a notification)
                throw new InvalidOperationException("Failed to log audit record", ex);
            }
        }

    }
}