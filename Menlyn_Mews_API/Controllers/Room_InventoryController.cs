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
using Twilio.TwiML.Voice;
using Menlyn_Mews_API.ViewModels.Inventory;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Room_InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Room_InventoryController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetRoomInventories")]
        public async Task<ActionResult> GetRoom_Inventory()
        {
            try
            {
                var results = await _repository.GetRoomInventoriesAsync();

                dynamic roomInventories = results.Select(ri => new
                {
                    ri.RoomId,
                    ri.InventoryId, 
                    Room_Number = ri.Room.Room_Number,
                    Supplied_By_Inventory = ri.Inventory.Inventory_Name,
                });

                return Ok(roomInventories); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRoomInventoryById/{roomId}/{inventoryId}")]
        public async Task<ActionResult> GetRoom_Inventory(int roomId, int inventoryId)
        {
            try
            {
                var ri = await _repository.GetRoomInventoryByIdAsync(roomId, inventoryId);
                if (ri == null) return NotFound("Room Inventory Does Not Exist");

                dynamic roomInventory = new
                {
                    ri.RoomId,
                    ri.InventoryId,
                };

                return Ok(roomInventory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRoomInventory/{roomId}/{inventoryId}")]
        public async Task<ActionResult<RoomInventoryViewModel>> PutRoom_Inventory(int roomId, int inventoryId, RoomInventoryViewModel rivm)
        {
            try
            {
                var ri = await _repository.GetRoomInventoryByIdAsync(roomId, inventoryId);
                if (ri == null) return NotFound("Room Inventory Does Not Exist");

                ri.RoomId = rivm.RoomId;
                ri.InventoryId = rivm.InventoryId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(ri);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AssignRoomInventory")]
        public async Task<IActionResult> PostRoom_Inventory(RoomInventoryViewModel rivm)
        {
            var roomInventory = new Room_Inventory
            {
                RoomId = rivm.RoomId,
                InventoryId = rivm.InventoryId, 
            };

            try
            {
                _repository.Add(roomInventory);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }
            return Ok(roomInventory);
        }

        // DELETE: api/Room_Inventory/5
        [HttpDelete("{roomId}/{inventoryId}")]
        public async Task<IActionResult> DeleteRoom_Inventory(int id, int id2)
        {
            if (_context.Room_Inventory == null)
            {
                return NotFound();
            }
            var room_Inventory = await _context.Room_Inventory.FindAsync(id, id2);
            if (room_Inventory == null)
            {
                return NotFound();
            }

            _context.Room_Inventory.Remove(room_Inventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Room_InventoryExists(int id)
        {
            return (_context.Room_Inventory?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
