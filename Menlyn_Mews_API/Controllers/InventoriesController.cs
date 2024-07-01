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
using Menlyn_Mews_API.Models.Repositories;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repositroy;

        public InventoriesController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repositroy = repositroy;
        }

        [HttpGet]
        [Route("GetInventories")]
        public async Task<ActionResult> GetInventories()
        {
            try
            {
                var results = await _repositroy.GetInventoriesAsync();

                dynamic inventories = results.Select(i => new
                {
                    i.InventoryId,
                    i.Inventory_Name,
                    i.Maximum_Stock,
                    i.Minimum_Stock,
                    i.Inventory_Condition,
                    i.Inventory_Status,
                    Category_Name = i.InventoryCategory.Inventory_Category_Name,
                    Type_Name = i.InventoryType.Inventory_Type_Name,
                    Room = i.Room,
                });

                return Ok(inventories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        [HttpPut]
        [Route("UpdateInventory/{inventoryId}")]
        public async Task<ActionResult<InventoryViewModel>> PutInventory(int inventoryId, InventoryViewModel ivm)
        {
            try
            {
                var inventory = await _repositroy.GetInventoryByIdAsync(inventoryId);
                if (inventory == null) return NotFound("Inventory Does Not Exist");   

                inventory.Inventory_Name = ivm.Inventory_Name;
                inventory.Maximum_Stock = ivm.Maximum_Stock;
                inventory.Minimum_Stock = ivm.Minimum_Stock;
                inventory.Inventory_Status = ivm.Inventory_Status;
                inventory.Inventory_Condition = ivm.Condition;
                inventory.InventoryTypeId  = ivm.InventoryTypeId;
                inventory.InventoryCategoryId = ivm.InventoryCategoryId;
                inventory.RoomId = ivm.RoomId;

                if (await _repositroy.SaveChangesAsync())
                {
                    return Ok(inventory);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent(); 
        }

        [HttpPost]
        [Route("AddInventory")]
        public async Task<IActionResult> PostInventory(InventoryViewModel ivm)
        {
            var inventory = new Inventory { 
                Inventory_Name = ivm.Inventory_Name, 
                Minimum_Stock = ivm.Minimum_Stock, 
                Maximum_Stock = ivm.Maximum_Stock, 
                Inventory_Condition = ivm.Condition,
                Inventory_Status = ivm.Inventory_Status,
                InventoryCategoryId = ivm.InventoryCategoryId, 
                InventoryTypeId = ivm.InventoryTypeId,
                RoomId = ivm.RoomId,
            };


            try
            {
                _repositroy.Add(inventory);
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
