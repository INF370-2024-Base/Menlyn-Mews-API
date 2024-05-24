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
    public class Room_BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Room_BookingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Room_Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room_Booking>>> GetRoom_Bookings()
        {
          if (_context.Room_Bookings == null)
          {
              return NotFound();
          }
            return await _context.Room_Bookings.ToListAsync();
        }

        // GET: api/Room_Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room_Booking>> GetRoom_Booking(int id)
        {
          if (_context.Room_Bookings == null)
          {
              return NotFound();
          }
            var room_Booking = await _context.Room_Bookings.FindAsync(id);

            if (room_Booking == null)
            {
                return NotFound();
            }

            return room_Booking;
        }

        // PUT: api/Room_Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom_Booking(int id, Room_Booking room_Booking)
        {
            if (id != room_Booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(room_Booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Room_BookingExists(id))
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

        // POST: api/Room_Booking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room_Booking>> PostRoom_Booking(Room_Booking room_Booking)
        {
          if (_context.Room_Bookings == null)
          {
              return Problem("Entity set 'AppDbContext.Room_Bookings'  is null.");
          }
            _context.Room_Bookings.Add(room_Booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom_Booking", new { id = room_Booking.Id }, room_Booking);
        }

        // DELETE: api/Room_Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom_Booking(int id)
        {
            if (_context.Room_Bookings == null)
            {
                return NotFound();
            }
            var room_Booking = await _context.Room_Bookings.FindAsync(id);
            if (room_Booking == null)
            {
                return NotFound();
            }

            _context.Room_Bookings.Remove(room_Booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Room_BookingExists(int id)
        {
            return (_context.Room_Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
