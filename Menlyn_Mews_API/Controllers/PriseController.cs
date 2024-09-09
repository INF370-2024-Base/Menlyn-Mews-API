using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public PriseController(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        // GET: api/Prise
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prise>>> Get()
        {
            var prises = await _context.Prises
                .FromSqlRaw("EXEC GetAllPrises")
                .ToListAsync();

            await _auditLogService.LogAsync("GET", "Prise", "GetAll", "Retrieved all prises");
            return Ok(prises);
        }

        // GET api/Prise/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prise>> Get(int id)
        {
            var prise = await _context.Prises
                .FromSqlRaw("EXEC GetPriseById @p0", id)
                .FirstOrDefaultAsync();

            if (prise == null)
            {
                await _auditLogService.LogAsync("GET", "Prise", $"GetById {id}", "Prise not found");
                return NotFound();
            }

            await _auditLogService.LogAsync("GET", "Prise", $"GetById {id}", $"Retrieved prise with ID {id}");
            return Ok(prise);
        }

        // POST: api/Prise
        [HttpPost]
        public async Task<ActionResult<Prise>> Post([FromBody] Prise prise)
        {
            try
            {
                // Check if the associated product exists
                var prod = await _context.Prods.FindAsync(prise.Prod_Id);
                if (prod == null)
                {
                    await _auditLogService.LogAsync("POST", "Prise", "Create", $"Product with ID {prise.Prod_Id} not found");
                    return NotFound($"Product with ID {prise.Prod_Id} not found");
                }

                // Add the prise if the product exists
                var newPriseId = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC CreatePrise @p0, @p1, @p2",
                    prise.Prices, prise.Date, prise.Prod_Id);

                // Log the action
                await _auditLogService.LogAsync("POST", "Prise", "Create", $"Created new prise with ID {newPriseId} for Product ID {prise.Prod_Id}");

                // Return the newly created prise
                var createdPrise = new Prise { Id = newPriseId, Prices = prise.Prices, Date = prise.Date, Prod_Id = prise.Prod_Id };
                return CreatedAtAction(nameof(Get), new { id = newPriseId }, createdPrise);
            }
            catch (Exception ex)
            {
                // Log the error
                await _auditLogService.LogAsync("POST", "Prise", "Error", $"Failed to create prise: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/Prise/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Prise updatedPrise)
        {
            if (id != updatedPrise.Id)
            {
                return BadRequest("Prise ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if the associated product exists
                var prod = await _context.Prods.FindAsync(updatedPrise.Prod_Id);
                if (prod == null)
                {
                    await _auditLogService.LogAsync("PUT", "Prise", $"Update {id}", $"Product with ID {updatedPrise.Prod_Id} not found");
                    return NotFound($"Product with ID {updatedPrise.Prod_Id} not found");
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdatePrise @p0, @p1, @p2, @p3",
                    id, updatedPrise.Prices, updatedPrise.Date, updatedPrise.Prod_Id);

                await _auditLogService.LogAsync("PUT", "Prise", $"Update {id}", $"Updated prise with ID {id} for Product ID {updatedPrise.Prod_Id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Prises.Any(p => p.Id == id))
                {
                    await _auditLogService.LogAsync("PUT", "Prise", $"Update {id}", "Prise not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/Prise/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var prise = await _context.Prises
                .FromSqlRaw("EXEC GetPriseById @p0", id)
                .FirstOrDefaultAsync();

            if (prise == null)
            {
                await _auditLogService.LogAsync("DELETE", "Prise", $"Delete {id}", "Prise not found");
                return NotFound();
            }

            await _context.Database.ExecuteSqlRawAsync("EXEC DeletePrise @p0", id);
            await _auditLogService.LogAsync("DELETE", "Prise", $"Delete {id}", $"Deleted prise with ID {id}");

            return NoContent();
        }
    }
}
