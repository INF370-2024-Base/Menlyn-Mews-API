namespace Menlyn_Mews_API.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(string action, string controllerName, string actionName, string details);

    }
}
