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
    public class InventoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryViewModel>>> GetInventories()
        {
          if (_context.Inventories == null || _context.Inventory_Types == null || _context.Inventory_Categories == null)
          {
              return NotFound();
          }
            var inventories = await _context.Inventories
                .Include(i => i.InventoryCategory)
                .Include(i => i.InventoryType)
                .Select(i => new InventoryViewModel
                {
                    Inventory_Name = i.Inventory_Name,
                    Minimum_Stock = (int)i.Minimum_Stock,
                    Maximum_Stock = (int)i.Maximum_Stock,
                    Condition = i.Inventory_Condition,
                    Inventory_Status = i.Inventory_Status,
                    InventoryCategoryId = i.InventoryCategory.InventoryCategoryId,
                    InventoryTypeId = i.InventoryType.InventoryTypeId,
                    InventoryCategoryName = i.InventoryCategory.Inventory_Category_Name,
                    InventoryTypeName = i.InventoryType.Inventory_Type_Name
                })
                .ToListAsync();

            return inventories;
        }

        // GET: api/Inventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
          if (_context.Inventories == null)
          {
              return NotFound();
          }
            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return inventory;
        }

        // PUT: api/Inventories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return BadRequest();
            }

            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
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

        // POST: api/Inventories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(AddInventoryViewModel ivm)
        {
            var inventory = new Inventory { 
                Inventory_Name = ivm.Inventory_Name, 
                Minimum_Stock = ivm.Minimum_Stock, 
                Maximum_Stock = ivm.Maximum_Stock, 
                InventoryCategoryId = ivm.InventoryCategoryId, 
                InventoryTypeId = ivm.InventoryTypeId };

            try
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();  
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }
            return Ok(inventory);
        }

        // DELETE: api/Inventories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            if (_context.Inventories == null)
            {
                return NotFound();
            }
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryExists(int id)
        {
            return (_context.Inventories?.Any(e => e.InventoryId == id)).GetValueOrDefault();
        }
    }
}
