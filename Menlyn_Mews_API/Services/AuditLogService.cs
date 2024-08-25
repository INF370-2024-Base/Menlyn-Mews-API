using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;

        public AuditLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string action, string controllerName, string actionName, string details)
        {
            var auditLog = new Audit_Log
            {
                User_Name = "Anonymous", // Replace with actual user information if available
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
