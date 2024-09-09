using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Prod_CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Prod_CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Prod_Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prod_Category>>> GetAllProdCategories()
        {
            try
            {
                var categories = await _context.Prod_Categories.ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Prod_Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prod_Category>> GetProdCategoryById(int id)
        {
            try
            {
                var category = await _context.Prod_Categories
                    .FirstOrDefaultAsync(pc => pc.Id == id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Prod_Category
        [HttpPost]
        public async Task<ActionResult<Prod_Category>> CreateProdCategory([FromBody] Prod_Category prodCategory)
        {
            if (prodCategory == null)
            {
                return BadRequest("ProdCategory is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Prod_Categories.Add(prodCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProdCategoryById), new { id = prodCategory.Id }, prodCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Prod_Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProdCategory(int id, [FromBody] Prod_Category prodCategory)
        {
            if (id != prodCategory.Id)
            {
                return BadRequest("Category ID mismatch.");
            }

            if (prodCategory == null)
            {
                return BadRequest("ProdCategory is null.");
            }

            try
            {
                _context.Entry(prodCategory).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Prod_Categories.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Prod_Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdCategory(int id)
        {
            try
            {
                var prodCategory = await _context.Prod_Categories.FindAsync(id);
                if (prodCategory == null)
                {
                    return NotFound();
                }

                _context.Prod_Categories.Remove(prodCategory);
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
