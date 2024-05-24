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
    public class Inventory_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Inventory_TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventory_Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory_Type>>> GetInventory_Types()
        {
          if (_context.Inventory_Types == null)
          {
              return NotFound();
          }
            return await _context.Inventory_Types.ToListAsync();
        }

        // GET: api/Inventory_Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory_Type>> GetInventory_Type(int id)
        {
          if (_context.Inventory_Types == null)
          {
              return NotFound();
          }
            var inventory_Type = await _context.Inventory_Types.FindAsync(id);

            if (inventory_Type == null)
            {
                return NotFound();
            }

            return inventory_Type;
        }

        // PUT: api/Inventory_Type/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory_Type(int id, Inventory_Type inventory_Type)
        {
            if (id != inventory_Type.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventory_Type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Inventory_TypeExists(id))
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

        // POST: api/Inventory_Type
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inventory_Type>> PostInventory_Type(Inventory_Type inventory_Type)
        {
          if (_context.Inventory_Types == null)
          {
              return Problem("Entity set 'AppDbContext.Inventory_Types'  is null.");
          }
            _context.Inventory_Types.Add(inventory_Type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventory_Type", new { id = inventory_Type.Id }, inventory_Type);
        }

        // DELETE: api/Inventory_Type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory_Type(int id)
        {
            if (_context.Inventory_Types == null)
            {
                return NotFound();
            }
            var inventory_Type = await _context.Inventory_Types.FindAsync(id);
            if (inventory_Type == null)
            {
                return NotFound();
            }

            _context.Inventory_Types.Remove(inventory_Type);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Inventory_TypeExists(int id)
        {
            return (_context.Inventory_Types?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
