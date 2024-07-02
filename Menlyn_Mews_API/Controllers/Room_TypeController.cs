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
    public class Room_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Room_TypeController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetRoomTypes")]
        public async Task<ActionResult> GetRoom_Types()
        {
            try
            {
                var roomTypes = await _repository.GetRoomTypesAsync();
                return Ok(roomTypes);   
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetRoomTypeById/{roomTypeId}")]
        public async Task<ActionResult> GetRoom_Type(int roomTypeId)
        {
            try
            {
                var roomTypes = await _repository.GetRoomTypeByIdAsync(roomTypeId);
                if (roomTypes == null) return NotFound("Room Type Does Not Exist");
                return Ok(roomTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRoomType/{roomTypeId}")]
        public async Task<ActionResult<RoomTypeViewModel>> PutRoom_Type(int roomTypeId, RoomTypeViewModel rvm)
        {
            try
            {
                var roomTypes = await _repository.GetRoomTypeByIdAsync(roomTypeId);
                if (roomTypes == null) return NotFound("Room Type Does Not Exist");

                roomTypes.Room_Type_Description = rvm.Room_Type_Description;
                roomTypes.Room_Type_Capacity = rvm.Room_Type_Capacity;  
                roomTypes.Room_Size = rvm.Room_Size;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(roomTypes);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddRoomType")]
        public async Task<IActionResult> PostRoom_Type(RoomTypeViewModel rvm)
        {
            var roomType = new Room_Type
            {
                Room_Type_Description = rvm.Room_Type_Description,
                Room_Type_Capacity = rvm.Room_Type_Capacity,
                Room_Size = rvm.Room_Size,
            };

            try
            {
                _repository.Add(roomType);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(roomType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom_Type(int id)
        {
            if (_context.Room_Types == null)
            {
                return NotFound();
            }
            var room_Type = await _context.Room_Types.FindAsync(id);
            if (room_Type == null)
            {
                return NotFound();
            }

            _context.Room_Types.Remove(room_Type);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
