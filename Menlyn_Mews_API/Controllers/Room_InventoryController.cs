using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
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

        [HttpGet]
        [Route("FilterInventories/{roomId}")]
        public async Task<IActionResult> FilterInventories(int roomId)
        {
            try
            {
                var roomInventories = await _repository.FilterInventoriesByRoomIdAsync(roomId);

                if (roomInventories == null)
                {
                    return NotFound("Room Does Not Have An Inventory Assigned");
                }

                var allInventories = await _repository.GetInventoriesAsync();

                var roomInventoryIds = roomInventories.Select(i => i.InventoryId).ToHashSet();

                var filteredInventories = allInventories.Where(i => roomInventoryIds.Contains(i.InventoryId));

                dynamic results = filteredInventories.Select(i => new
                {
                    i.InventoryId,
                    i.Inventory_Name
                });

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetInventoriesNotAssignedToRoom/{roomId}")]
        public async Task<IActionResult> GetInventoriesNotAssignedToRoom(int roomId)
        {
            try
            {
                var roomInventories = await _repository.FilterInventoriesByRoomIdAsync(roomId);

                if (roomInventories == null)
                {
                    return NotFound("Room Does Not Have An Inventory Assigned");
                }

                var allInventories = await _repository.GetInventoriesAsync();

                var roomInventoryIds = roomInventories.Select(i => i.InventoryId).ToHashSet();

                var filteredInventories = allInventories.Where(i => !roomInventoryIds.Contains(i.InventoryId));

                dynamic results = filteredInventories.Select(i => new
                {
                    i.InventoryId,
                    i.Inventory_Name
                });

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("ViewRoomInventory/{roomId}")]
        public async Task<IActionResult> GetRoomsInventory(int roomId)
        {
            try
            {
                var results = await _repository.FilterInventoriesByRoomIdAsync(roomId);

                if (results == null || !results.Any())
                {
                    return NotFound("Room Inventory Does Not Exist");
                }

                var roomInventory = results
                    .Where(r => r.RoomId == roomId)
                    .GroupBy(r => r.RoomId)
                    .Select(g => new
                    {
                        RoomId = g.Key, 
                        Inventories = string.Join(", ", g.Select(i => $"{i.Inventory.Inventory_Name}"))
                    })
                    .FirstOrDefault();

                return Ok(roomInventory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("{roomId}/{inventoryId}")]
        public async Task<IActionResult> DeleteRoom_Inventory(int roomId, int inventoryId)
        {
            if (_context.Room_Inventory == null)
            {
                return NotFound();
            }
            var room_Inventory = await _context.Room_Inventory.FindAsync(roomId, inventoryId);
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
