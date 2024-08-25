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
using Menlyn_Mews_API.Models.Repositories;


namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Room_BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        private string accountSid = "AC68ce8e5c11a913eb26d112a30b19aabb";
        private string authToken = "a88822c4277482823eef8666a52998c3";

        public Room_BookingController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        public async Task<ActionResult> GetRoom_Bookings()
        {
            try
            {
                var results = await _repository.GetRoomBookingsAsync();

                dynamic roomBookings = results.Select(rb => new
                {
                    rb.RoomBookingId,
                    rb.Check_In_Date,
                    rb.Check_Out_Date,
                    rb.Booking_Status,
                    rb.Booking_Price,
                    rb.Is_Reviewed,
                    Client = rb.Clients?.Client_Name + " " + rb.Clients?.Client_Surname,
                    Room_Desc = rb.Rooms?.Room_Description,
                    Room_Floor = rb.Rooms?.Room_Floor,
                    Booking_Package = rb.Booking_Package?.Booking_Package_Description,
                    Discount = rb.Discount?.Discount_Name,
                });

                return Ok(roomBookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBookedRooms/{roomId}")]
        public async Task<ActionResult> GetBookedRooms(int roomId)
        {
            try
            {
                var results = await _repository.GetBookedRooms(roomId);

                dynamic roomBookings = results.Select(rb => new
                {
                    Check_In_Date = rb.Check_In_Date.Date.ToString("yyyy-MM-dd"),
                    Check_Out_Date = rb.Check_Out_Date.Date.ToString("yyyy-MM-dd"),
                    rb.Booking_Status,
                    rb.Is_Reviewed,
                });

                return Ok(roomBookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRoomBookingById/{roomBookingId}")]
        public async Task<ActionResult> GetRoom_Booking(int roomBookingId)
        {
            try
            {
                var rb = await _repository.GetRoomBookingByIdAsync(roomBookingId);
                if (rb == null) return NotFound("Room Booking Does Not Exist");

                dynamic roomBookings = new
                {
                    rb.RoomBookingId,
                    rb.Check_In_Date,
                    rb.Check_Out_Date,
                    rb.Booking_Status,
                    rb.Booking_Price,
                    rb.ClientId,
                    rb.RoomId,
                    rb.BookingPackageId,
                    rb.DiscountId,
                    rb.Is_Reviewed
                };

                return Ok(roomBookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]//Booking History
        [Route("GetRoomBookingByClientId/{clientId}")]
        public async Task<ActionResult> GetRoom_BookingClient(int clientId)
        {
            try
            {
                var results = await _repository.GetRoomBookingByClientIdAsync(clientId);
                if (results == null) return NotFound("You Do Not Have Any Bookings!");

                dynamic roomBookings = results.Select(rb => new
                {
                    rb.RoomBookingId,
                    rb.Check_In_Date,
                    rb.Check_Out_Date,
                    Check_Out_String = rb.Check_Out_Date.ToString("yyyy-MM-dd"),
                    rb.Booking_Status,
                    rb.Booking_Price,
                    Client = rb.Clients?.Client_Name + " " + rb.Clients?.Client_Surname,
                    Room_Desc = rb.Rooms?.Room_Description,
                    rb.Rooms!.Room_Number,
                    Booking_Package = rb.Booking_Package?.Booking_Package_Description,
                    Discount = rb.Discount?.Discount_Name,
                    rb.Is_Reviewed,
                    rb.ClientId,
                    rb.RoomId,
                    rb.Booking_Review?.Review_Rating,
                    rb.Booking_Review?.Review_Description,
                })
                .OrderByDescending(rb => rb.Check_Out_Date);

                return Ok(roomBookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetLatestBooking/{clientId}")]
        public async Task<ActionResult> GetLatestBooking(int clientId)
        {
            try
            {
                var results = await _repository.GetRoomBookingByClientIdAsync(clientId);
                if (results == null) return NotFound("You Do Not Have Any Bookings!");

                dynamic roomBookings = results.OrderByDescending(rb => rb.Check_Out_Date).Where(rb => rb.Is_Reviewed == false).Select(rb => new
                {
                    rb.RoomBookingId,
                    rb.Check_Out_Date,
                    Check_Out_String = rb.Check_Out_Date.ToString("yyyy-MM-dd"),
                    rb.Booking_Price,
                    Client = rb.Clients?.Client_Name + " " + rb.Clients?.Client_Surname,
                    Room_Desc = rb.Rooms?.Room_Description,
                    rb.RoomId,
                    rb.Rooms.Room_Number,
                    rb.Is_Reviewed,
                })
                .FirstOrDefault();

                if (roomBookings == null) return NotFound("No latest booking found!");

                var respone = new[] { roomBookings };

                return Ok(respone);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("UpdateRoomBooking/{roomBookingId}")]
        public async Task<ActionResult<BookingViewModel>> PutRoom_Booking(int roomBookingId, BookingViewModel bvm)
        {
            try
            {
                var rb = await _repository.GetRoomBookingByIdAsync(roomBookingId);
                if (rb == null) return NotFound("Room Booking Does Not Exist");


                rb.Check_In_Date = bvm.Check_In_Date.GetValueOrDefault();
                rb.Check_Out_Date = bvm.Check_Out_Date.GetValueOrDefault();
                rb.Booking_Status = bvm.Booking_Status;
                rb.Booking_Price = bvm.Booking_Price;
                rb.ClientId = bvm.ClientId;
                rb.RoomId = bvm.RoomId;
                rb.BookingPackageId = bvm.BookingPackageId;
                if (rb.DiscountId == -1)
                {
                    rb.DiscountId = null;
                }
                else
                {
                    rb.DiscountId = bvm.DiscountId;
                }                

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(rb);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("CreateRoomBooking")]
        public async Task<IActionResult> PostRoom_Booking(BookingViewModel bvm)
        {
            var vat = await _repository.GetVATAsync();

            var booking = new Room_Booking
            {
                Check_In_Date = bvm.Check_In_Date.GetValueOrDefault(),
                Check_Out_Date = bvm.Check_Out_Date.GetValueOrDefault(),
                Booking_Price = bvm.Booking_Price + (bvm.Booking_Price * vat.FirstOrDefault().VAT_Amount / 100),
                Booking_Status = bvm.Booking_Status,
                Is_Reviewed = false,
                ClientId = bvm.ClientId,
                RoomId = bvm.RoomId,
                BookingPackageId = bvm.BookingPackageId,
                DiscountId = bvm.DiscountId,
            };
            if (booking.DiscountId == -1)
            {
                booking.DiscountId = null;
            }
            try
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                //TwilioClient.Init(accountSid, authToken); /*ONLY UNCOMMENT FOR PRESENTATIONS, COSTS MONEY TO USE*/

                //var message = MessageResource.Create(
                //    body: "Your Check In Date Is: " + booking.Check_In_Date + " Your Check Out Date Is: " + booking.Check_Out_Date + " The Cost Of Your Booking Is: R" + booking.Booking_Price + " . Menlyn Mews, Where Your Mews Is Our Menlyn",
                //    from: new Twilio.Types.PhoneNumber("+13187034034"),
                //    to: new Twilio.Types.PhoneNumber("+27646028374")
                //);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(booking);
        }

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
