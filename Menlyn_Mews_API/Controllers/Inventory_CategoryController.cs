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
    public class Inventory_CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repositroy;

        public Inventory_CategoryController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repositroy = repositroy;
        }

        [HttpGet]
        [Route("GetInventoryCategories")]
        public async Task<ActionResult> GetInventory_Categories()
        {
            try
            {
                var inventoryCategories = await _repositroy.GetInventoryCategoriesAsync();
                return Ok(inventoryCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInventoryCategoryById/{inventoryCategoryId}")]
        public async Task<ActionResult<Inventory_Category>> GetInventory_Category(int inventoryCategoryId)
        {
            try
            {
                var inventoryCategories = await _repositroy.GetInventoryCategoriesByIdAsync(inventoryCategoryId);
                return Ok(inventoryCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateInventoryCategory/{inventoryCategoryId}")]
        public async Task<ActionResult<InventoryCategoryViewModel>> PutInventory_Category(int inventoryCategoryId, InventoryCategoryViewModel icvm)
        {
            try
            {
                var inventoryCategory = await _repositroy.GetInventoryCategoriesByIdAsync(inventoryCategoryId);
                if (inventoryCategory == null) return NotFound("Inventory Category Does Not Exist");

                inventoryCategory.Inventory_Category_Name = icvm.Inventory_Category_Name;
                inventoryCategory.Inventory_Category_Description = icvm.Inventory_Category_Description;

                if (await _repositroy.SaveChangesAsync())
                {
                    return Ok(inventoryCategory);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddInventoryCategory")]
        public async Task<IActionResult> PostInventory_Category(InventoryCategoryViewModel icvm)
        {
            var inventory_category = new Inventory_Category 
            { 
                Inventory_Category_Name = icvm.Inventory_Category_Name, 
                Inventory_Category_Description = icvm.Inventory_Category_Description 
            };

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
