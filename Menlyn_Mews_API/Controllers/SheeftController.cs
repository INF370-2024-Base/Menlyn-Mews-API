using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Data;
using Microsoft.EntityFrameworkCore;


namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheeftController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SheeftController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sheeft
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sheeft>>> GetSheefts()
        {
            return await _context.Sheefts.ToListAsync();
        }

        // GET: api/Sheeft/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sheeft>> GetSheeft(int id)
        {
            var sheeft = await _context.Sheefts.FindAsync(id);

            if (sheeft == null)
            {
                return NotFound();
            }

            return sheeft;
        }

        // POST: api/Sheeft
        [HttpPost]
        public async Task<ActionResult<Sheeft>> PostSheeft(Sheeft sheeft)
        {
            _context.Sheefts.Add(sheeft);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSheeft), new { id = sheeft.Id }, sheeft);
        }

        // PUT: api/Sheeft/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSheeft(int id, Sheeft sheeft)
        {
            if (id != sheeft.Id)
            {
                return BadRequest();
            }

            _context.Entry(sheeft).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SheeftExists(id))
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

        // DELETE: api/Sheeft/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSheeft(int id)
        {
            var sheeft = await _context.Sheefts.FindAsync(id);
            if (sheeft == null)
            {
                return NotFound();
            }

            _context.Sheefts.Remove(sheeft);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SheeftExists(int id)
        {
            return _context.Sheefts.Any(e => e.Id == id);
        }
    }
}
