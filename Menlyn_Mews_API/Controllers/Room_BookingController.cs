using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Menlyn_Mews_API.ViewModels.Booking;


namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Room_BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        string accountSid = "AC68ce8e5c11a913eb26d112a30b19aabb";
        string authToken = "a88822c4277482823eef8666a52998c3";

        public Room_BookingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room_Booking>>> GetRoom_Bookings()
        {
          if (_context.Room_Bookings == null)
          {
              return NotFound();
          }
            return await _context.Room_Bookings.ToListAsync();
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom_Booking(int id, Room_Booking room_Booking)
        {
            if (id != room_Booking.RoomBookingId)
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

        [HttpPost]
        public async Task<ActionResult<Room_Booking>> PostRoom_Booking(BookingViewModel bvm)
        {
            var booking = new Room_Booking
            {
                Check_In_Date =  bvm.Check_In_Date,
                Check_Out_Date = bvm.Check_Out_Date,
                Booking_Price  = bvm.Booking_Price,
                ClientId = bvm.ClientId,
                RoomId = bvm.Room_Id,
            };

            try
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "Your Check In Date Is " + booking.Check_In_Date,
                    from: new Twilio.Types.PhoneNumber("+13187034034"),
                    to: new Twilio.Types.PhoneNumber("+27646028374")
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(booking);
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
            return (_context.Room_Bookings?.Any(e => e.RoomBookingId == id)).GetValueOrDefault();
        }
    }
}
