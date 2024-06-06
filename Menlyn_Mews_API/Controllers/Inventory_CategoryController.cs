using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.ViewModels;

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
        public async Task<ActionResult<IEnumerable<InventoryCategoryViewModel>>> GetInventory_Categories()
        {
          if (_context.Inventory_Categories == null || _context.Inventories == null)
          {
              return NotFound();
          }
           
          var inventory_categories = await _context.Inventory_Categories
                .Select(i => new InventoryCategoryViewModel {
                    Inventory_Category_Name = i.Inventory_Category_Name,
                    Inventory_Category_Description = i.Inventory_Category_Description
                })
                .ToListAsync();

            return inventory_categories;
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
            if (id != inventory_Category.InventoryCategoryId)
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

        [HttpPost]
        public async Task<ActionResult<Inventory_Category>> PostInventory_Category(InventoryCategoryViewModel icvm)
        {
            var inventory_category = new Inventory_Category { Inventory_Category_Name = icvm.Inventory_Category_Name, Inventory_Category_Description = icvm.Inventory_Category_Description };

            try
            {
                _context.Add(inventory_category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }
            return Ok(inventory_category);
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
            return (_context.Inventory_Categories?.Any(e => e.InventoryCategoryId == id)).GetValueOrDefault();
        }
    }
}
