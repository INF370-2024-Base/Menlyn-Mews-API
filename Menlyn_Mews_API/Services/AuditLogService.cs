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
            var isAuthenticated = httpContext?.User?.Identity?.IsAuthenticated == true;

            // If the user is authenticated, use their claims to log the user; otherwise, log as "John Smith"
            var userName = isAuthenticated
                           ? httpContext.User.Identity.Name
                           : "Ammar Ulhaq"; // Log as John Smith if user is not authenticated

            // Extract Employee name and surname from claims, if available
            var employeeName = isAuthenticated ? httpContext?.User?.FindFirst("EmployeeName")?.Value : "John";
            var employeeSurname = isAuthenticated ? httpContext?.User?.FindFirst("EmployeeSurname")?.Value : "Smith";

            // Create a new audit log entry
            var auditLog = new Audit_Log
            {
                User_Name = $" {userName}", // Combine name and surname
                Action = action,
                Controller_Name = controllerName,
                Action_Name = actionName,
                Timestamp = DateTime.UtcNow,
                Details = details
            };

            // Save the audit log entry to the database
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