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
    public class Inventory_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Inventory_TypeController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetInventoryTypes")]
        public async Task<ActionResult> GetInventory_Types()
        {
            try
            {
                var inventoryTypes = await _repository.GetInventoryTypesAsync();
                return Ok(inventoryTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInventoryTypeById/{inventoryTypeId}")]
        public async Task<ActionResult<Inventory_Type>> GetInventory_Type(int inventoryTypeId)
        {
            try
            {
                var inventoryTypes = await _repository.GetInventoryTypesByIdAsync(inventoryTypeId);
                return Ok(inventoryTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateInventoryType/{inventoryTypeId}")]
        public async Task<ActionResult<InventoryTypeViewModel>> PutInventory_Type(int inventoryTypeId, InventoryTypeViewModel itvm)
        {
            try
            {
                var inventoryTypes = await _repository.GetInventoryTypesByIdAsync(inventoryTypeId);
                if (inventoryTypes == null) return NotFound("Inventory Type Does Not Exist");

                inventoryTypes.Inventory_Type_Name = itvm.Inventory_Type_Name;
                inventoryTypes.Inventory_Type_Description = itvm.Inventory_Type_Description;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(inventoryTypes);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddInventoryType")]
        public async Task<IActionResult> PostInventory_Type(InventoryTypeViewModel itvm)
        {
            var inventory_type = new Inventory_Type 
                { 
                    Inventory_Type_Name = itvm.Inventory_Type_Name, 
                    Inventory_Type_Description = itvm.Inventory_Type_Description 
                };

            try
            {
                _repository.Add(inventory_type);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }
            return Ok(inventory_type);
        }

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
