using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Room_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Room_TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Room_Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room_Type>>> GetRoom_Types()
        {
          if (_context.Room_Types == null)
          {
              return NotFound();
          }
            return await _context.Room_Types.ToListAsync();
        }

        // GET: api/Room_Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room_Type>> GetRoom_Type(int id)
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

            return room_Type;
        }

        // PUT: api/Room_Type/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom_Type(int id, Room_Type room_Type)
        {
            if (id != room_Type.RoomTypeId)
            {
                return BadRequest();
            }

            _context.Entry(room_Type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Room_TypeExists(id))
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

        // POST: api/Room_Type
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room_Type>> PostRoom_Type(Room_Type room_Type)
        {
          if (_context.Room_Types == null)
          {
              return Problem("Entity set 'AppDbContext.Room_Types'  is null.");
          }
            _context.Room_Types.Add(room_Type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom_Type", new { id = room_Type.RoomTypeId }, room_Type);
        }

        // DELETE: api/Room_Type/5
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

        private bool Room_TypeExists(int id)
        {
            return (_context.Room_Types?.Any(e => e.RoomTypeId == id)).GetValueOrDefault();
        }
    }
}
