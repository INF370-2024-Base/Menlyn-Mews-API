using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.ViewModels.Inventory;

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
        public async Task<ActionResult<IEnumerable<InventoryTypeViewModel>>> GetInventory_Types()
        {
            if (_context.Inventory_Types == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory_types = await _context.Inventory_Types
                  .Select(i => new InventoryTypeViewModel
                  {
                      Inventory_Type_Name = i.Inventory_Type_Name,
                      Inventory_Type_Description = i.Inventory_Type_Description
                  })
                  .ToListAsync();

            return inventory_types;
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
            if (id != inventory_Type.InventoryTypeId)
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
        public async Task<ActionResult<Inventory_Type>> PostInventory_Type(InventoryTypeViewModel itvm)
        {
            var inventory_type = new Inventory_Type { Inventory_Type_Name = itvm.Inventory_Type_Name, Inventory_Type_Description = itvm.Inventory_Type_Description };

            try
            {
                _context.Add(inventory_type);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }
            return Ok(inventory_type);
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
            return (_context.Inventory_Types?.Any(e => e.InventoryTypeId == id)).GetValueOrDefault();
        }
    }
}
