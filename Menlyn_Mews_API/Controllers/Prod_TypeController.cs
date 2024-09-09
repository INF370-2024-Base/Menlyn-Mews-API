using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Prod_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Prod_TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Prod_Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prod_Type>>> Get()
        {
            try
            {
                var prodTypes = await _context.Prod_Types.ToListAsync();
                return Ok(prodTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Prod_Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prod_Type>> Get(int id)
        {
            try
            {
                var prodType = await _context.Prod_Types.FindAsync(id);

                if (prodType == null)
                {
                    return NotFound();
                }

                return Ok(prodType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Prod_Type
        [HttpPost]
        public async Task<ActionResult<Prod_Type>> Post([FromBody] Prod_Type prodType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Prod_Types.Add(prodType);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = prodType.Id }, prodType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Prod_Type/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Prod_Type prodType)
        {
            if (id != prodType.Id)
            {
                return BadRequest("Product Type ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(prodType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Prod_Types.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Prod_Type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var prodType = await _context.Prod_Types.FindAsync(id);
                if (prodType == null)
                {
                    return NotFound();
                }

                _context.Prod_Types.Remove(prodType);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
