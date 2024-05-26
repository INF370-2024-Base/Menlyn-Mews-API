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
    public class Booking_ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Booking_ReviewController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Booking_Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking_Review>>> GetBooking_Reviews()
        {
          if (_context.Booking_Reviews == null)
          {
              return NotFound();
          }
            return await _context.Booking_Reviews.ToListAsync();
        }

        // GET: api/Booking_Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking_Review>> GetBooking_Review(int id)
        {
          if (_context.Booking_Reviews == null)
          {
              return NotFound();
          }
            var booking_Review = await _context.Booking_Reviews.FindAsync(id);

            if (booking_Review == null)
            {
                return NotFound();
            }

            return booking_Review;
        }

        // PUT: api/Booking_Review/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking_Review(int id, Booking_Review booking_Review)
        {
            if (id != booking_Review.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking_Review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Booking_ReviewExists(id))
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

        // POST: api/Booking_Review
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking_Review>> PostBooking_Review(Booking_Review booking_Review)
        {
          if (_context.Booking_Reviews == null)
          {
              return Problem("Entity set 'AppDbContext.Booking_Reviews'  is null.");
          }
            _context.Booking_Reviews.Add(booking_Review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking_Review", new { id = booking_Review.Id }, booking_Review);
        }

        // DELETE: api/Booking_Review/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking_Review(int id)
        {
            if (_context.Booking_Reviews == null)
            {
                return NotFound();
            }
            var booking_Review = await _context.Booking_Reviews.FindAsync(id);
            if (booking_Review == null)
            {
                return NotFound();
            }

            _context.Booking_Reviews.Remove(booking_Review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Booking_ReviewExists(int id)
        {
            return (_context.Booking_Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
