using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Filters;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menlyn_Mews_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(AuditLogFilter))]  // Apply the custom action filter
    public class AuditLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditLogController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AuditLog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Audit_Log>>> GetAuditLogs()
        {
            return await _context.AuditLogs.ToListAsync();
        }

        // GET: api/AuditLog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Audit_Log>> GetAuditLog(int id)
        {
            var auditLog = await _context.AuditLogs.FindAsync(id);

            if (auditLog == null)
            {
                return NotFound();
            }

            return auditLog;
        }

        // POST: api/AuditLog
        [HttpPost]
        public async Task<ActionResult<Audit_Log>> PostAuditLog(Audit_Log auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuditLog", new { id = auditLog.Id }, auditLog);
        }

        // PUT: api/AuditLog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuditLog(int id, Audit_Log auditLog)
        {
            if (id != auditLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(auditLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/AuditLog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuditLog(int id)
        {
            var auditLog = await _context.AuditLogs.FindAsync(id);
            if (auditLog == null)
            {
                return NotFound();
            }

            _context.AuditLogs.Remove(auditLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuditLogExists(int id)
        {
            return _context.AuditLogs.Any(e => e.Id == id);
        }
    }
}
