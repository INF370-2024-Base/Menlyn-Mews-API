using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Inventory;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Inspection_ItemController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Inspection_ItemController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetInspectionItems")]
        public async Task<ActionResult> GetInspection_Item()
        {
            try
            {
                var results = await _repository.GetInspectionItemsAsync();

                dynamic inspectionItems = results.Select(ii => new
                {
                    ii.InspectionItemId,
                    ii.Inspection_Item_Name,
                    ii.Inspection_Item_Condition,
                    Inventory_Name = ii.Inventory.Inventory_Name,
                });
                
                return Ok(inspectionItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInspectionItemById/{inspectionItemId}")]
        public async Task<ActionResult> GetInspection_Item(int inspectionItemId)
        {
            try
            {
                var ii = await _repository.GetInspectionItemsByIdAsync(inspectionItemId);

                dynamic inspectionItems = new
                {
                    ii.InspectionItemId,
                    ii.Inspection_Item_Name,
                    ii.Inspection_Item_Condition,
                    Inventory_Name = ii.Inventory.Inventory_Name,
                };

                return Ok(inspectionItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateInspectionItem/{inspectionItemId}")]
        public async Task<ActionResult<InspectionItemViewModel>> PutInspection_Item(int inspectionItemId, InspectionItemViewModel iivm)
        {
            try
            {
                var inspectionItem = await _repository.GetInspectionItemsByIdAsync(inspectionItemId);
                if (inspectionItem == null) return NotFound("Inspection Item Does Not Exist");

                inspectionItem.Inspection_Item_Name = iivm.Inspection_Item_Name;
                inspectionItem.Inspection_Item_Condition = iivm.Inspection_Item_Condition;
                inspectionItem.InventoryId = iivm.InventoryId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(inspectionItem);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddInspectionItem")]
        public async Task<IActionResult> PostInspection_Item(InspectionItemViewModel iivm)
        {

            var inspectionItem = new Inspection_Item
            {
                Inspection_Item_Name = iivm.Inspection_Item_Name,
                Inspection_Item_Condition = iivm.Inspection_Item_Condition,
                InventoryId = iivm.InventoryId,
            };

            try
            {
                _repository.Add(inspectionItem);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }

            return Ok(inspectionItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection_Item(int id)
        {
            if (_context.Inspection_Items == null)
            {
                return NotFound();
            }
            var inspection_Item = await _context.Inspection_Items.FindAsync(id);
            if (inspection_Item == null)
            {
                return NotFound();
            }

            _context.Inspection_Items.Remove(inspection_Item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Inspection_ItemExists(int id)
        {
            return (_context.Inspection_Items?.Any(e => e.InspectionItemId == id)).GetValueOrDefault();
        }
    }
}
