using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Prod
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prod>>> GetProds()
        {
            var prods = await _context.Prods.ToListAsync();
            return Ok(prods);
        }

        // GET api/Prod/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prod>> GetProd(int id)
        {
            var prod = await _context.Prods.FindAsync(id);

            if (prod == null)
            {
                return NotFound();
            }

            return Ok(prod);
        }

        // POST api/Prod
        [HttpPost]
        public async Task<ActionResult<Prod>> PostProd([FromBody] Prod prod)
        {
            _context.Prods.Add(prod);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProd), new { id = prod.Id }, prod);
        }

        // PUT api/Prod/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProd(int id, [FromBody] Prod prod)
        {
            if (id != prod.Id)
            {
                return BadRequest();
            }

            _context.Entry(prod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Prods.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE api/Prod/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProd(int id)
        {
            var prod = await _context.Prods.FindAsync(id);
            if (prod == null)
            {
                return NotFound();
            }

            _context.Prods.Remove(prod);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
