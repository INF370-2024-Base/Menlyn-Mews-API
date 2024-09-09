using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pos>>> GetPositions()
        {
            return await _context.Poss.ToListAsync(); // Changed to match DbSet name
        }

        // GET: api/Pos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pos>> GetPosition(int id)
        {
            var pos = await _context.Poss.FindAsync(id); // Changed to match DbSet name

            if (pos == null)
            {
                return NotFound();
            }

            return pos;
        }

        // POST: api/Pos
        [HttpPost]
        public async Task<ActionResult<Pos>> PostPosition(Pos pos)
        {
            _context.Poss.Add(pos); // Changed to match DbSet name
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPosition), new { id = pos.Id }, pos);
        }

        // PUT: api/Pos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, Pos pos)
        {
            if (id != pos.Id)
            {
                return BadRequest();
            }

            _context.Entry(pos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PosExists(id))
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

        // DELETE: api/Pos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var pos = await _context.Poss.FindAsync(id); // Changed to match DbSet name
            if (pos == null)
            {
                return NotFound();
            }

            _context.Poss.Remove(pos); // Changed to match DbSet name
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PosExists(int id)
        {
            return _context.Poss.Any(e => e.Id == id); // Changed to match DbSet name
        }
    }
}
