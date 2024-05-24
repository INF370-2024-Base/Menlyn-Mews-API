using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Inventory_CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Inventory_CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventory_Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory_Category>>> GetInventory_Categories()
        {
          if (_context.Inventory_Categories == null)
          {
              return NotFound();
          }
            return await _context.Inventory_Categories.ToListAsync();
        }

        // GET: api/Inventory_Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory_Category>> GetInventory_Category(int id)
        {
          if (_context.Inventory_Categories == null)
          {
              return NotFound();
          }
            var inventory_Category = await _context.Inventory_Categories.FindAsync(id);

            if (inventory_Category == null)
            {
                return NotFound();
            }

            return inventory_Category;
        }

        // PUT: api/Inventory_Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory_Category(int id, Inventory_Category inventory_Category)
        {
            if (id != inventory_Category.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventory_Category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Inventory_CategoryExists(id))
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

        // POST: api/Inventory_Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inventory_Category>> PostInventory_Category(Inventory_Category inventory_Category)
        {
          if (_context.Inventory_Categories == null)
          {
              return Problem("Entity set 'AppDbContext.Inventory_Categories'  is null.");
          }
            _context.Inventory_Categories.Add(inventory_Category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventory_Category", new { id = inventory_Category.Id }, inventory_Category);
        }

        // DELETE: api/Inventory_Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory_Category(int id)
        {
            if (_context.Inventory_Categories == null)
            {
                return NotFound();
            }
            var inventory_Category = await _context.Inventory_Categories.FindAsync(id);
            if (inventory_Category == null)
            {
                return NotFound();
            }

            _context.Inventory_Categories.Remove(inventory_Category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Inventory_CategoryExists(int id)
        {
            return (_context.Inventory_Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
