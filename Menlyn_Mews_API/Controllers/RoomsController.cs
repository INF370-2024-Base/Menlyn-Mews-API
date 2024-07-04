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
using Menlyn_Mews_API.ViewModels.Booking;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        
        public RoomsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetRooms")]
        public async Task<ActionResult> GetRooms()
        {
            try
            {
                var results = await _repository.GetRoomsAsync();

                dynamic rooms = results.Select(r => new
                {
                    r.RoomId,
                    r.Room_Number,
                    r.Room_Floor,
                    r.Room_Status,
                    r.Room_Rate,
                    r.Room_Description,
                    Room_Type = r.Room_Type.Room_Type_Description,
                });

                return Ok(rooms);   

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetRoomById/{roomId}")]
        public async Task<ActionResult> GetRoom(int roomId)
        {
            try
            {
                var r = await _repository.GetRoomByIdAsync(roomId);
                if (r == null) return NotFound("Room Does Not Exist");

                dynamic rooms = new
                {
                    r.RoomId,
                    r.Room_Number,
                    r.Room_Floor,
                    r.Room_Status,
                    r.Room_Rate,
                    r.Room_Description,
                    Room_Type = r.Room_Type.Room_Type_Description,
                };

                return Ok(rooms);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRoom/{roomId}")]
        public async Task<ActionResult<RoomViewModel>> PutRoom(int roomId, RoomViewModel rvm)
        {
            try
            {
                var r = await _repository.GetRoomByIdAsync(roomId);
                if (r == null) return NotFound("Room Does Not Exist");

                r.Room_Number = rvm.Room_Number;
                r.Room_Floor = rvm.Room_Floor;
                r.Room_Status = rvm.Room_Status;
                r.Room_Rate = rvm.Room_Rate;
                r.Room_Description = rvm.Room_Description;
                r.RoomTypeId = rvm.RoomTypeId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(r);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddRoom")]
        public async Task<IActionResult> PostRoom(RoomViewModel rvm)
        {
            var room = new Room
            {
                Room_Number = rvm.Room_Number,  
                Room_Floor = rvm.Room_Floor,
                Room_Status = rvm.Room_Status,
                Room_Rate = rvm.Room_Rate,
                Room_Description = rvm.Room_Description,
                RoomTypeId = rvm.RoomTypeId,
            };

            try
            {
                _repository.Add(room);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
