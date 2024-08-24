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
using Menlyn_Mews_API.ViewModels.Client;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Booking_ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Booking_ReviewController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetBookingReviews")]
        public async Task<ActionResult> GetBooking_Reviews()
        {
            try
            {
                var results = await _repository.GetBookingReviewsAsync();

                dynamic bookingReviews = results.Select(br => new
                {
                    br.BookingReviewId,
                    br.Review_Status,
                    br.Review_Rating,
                    br.Review_Description,
                    br.Date_Created,
                    Client = br.Client.Client_Name + " " + br.Client.Client_Surname,
                    Room = br.Room.Room_Number, 
                });

                return Ok(bookingReviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBookingReviewById/{bookingReviewId}")]
        public async Task<ActionResult> GetBooking_Review(int bookingReviewId)
        {
            try
            {
                var br = await _repository.GetBookingReviewByIdAsync(bookingReviewId);
                if (br == null) return NotFound("Booking Review Does Not Exist");


                dynamic bookingReviews = new
                {
                    br.BookingReviewId,
                    br.Review_Status,
                    br.Review_Rating,
                    br.Review_Description,
                    br.Date_Created,
                    br.ClientId,
                    br.RoomId   
                };

                return Ok(bookingReviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBookingReviewByRoomId/{roomId}")]
        public async Task<ActionResult> GetBooking_ReviewRooms(int roomId)
        {
            try
            {
                var br = await _repository.GetReviewsByRoomIdAsync(roomId);
                if (br == null) return NotFound("Booking Review Does Not Exist");


                dynamic bookingReviews = br.Select(br => new
                {
                    br.BookingReviewId,
                    br.Review_Status,
                    br.Review_Rating,
                    br.Review_Description,
                    br.Date_Created,
                    Username = br.Client.ApplicationUser.UserName,
                });

                return Ok(bookingReviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRoomRatingAverage/{roomId}")]
        public async Task<ActionResult> GetBooking_ReviewRoomsAverage(int roomId)
        {
            try
            {
                var br = await _repository.GetReviewsByRoomIdAsync(roomId);
                if (br == null) return NotFound("Booking Review Does Not Exist");


                var average = br
                    .Average(bs => bs.Review_Rating);

                return Ok( new 
                {
                    Average_Rating = Math.Round((decimal)average, 1)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("UpdateBookingReview/{bookingReviewId}")]
        public async Task<ActionResult<BookingReviewViewModel>> PutBooking_Review(int bookingReviewId, BookingReviewViewModel bvm)
        {
            try
            {
                var br = await _repository.GetBookingReviewByIdAsync(bookingReviewId);
                if (br == null) return NotFound("Booking Review Does Not Exist");

                br.Review_Status = bvm.Review_Status;   
                br.Review_Rating = bvm.Review_Rating;   
                br.Review_Description = bvm.Review_Description; 
                br.ClientId = bvm.ClientId;
                br.RoomId = bvm.RoomId;
                br.Date_Created = DateTime.Now;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(br);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostBooking_Review(BookingReviewViewModel bvm)
        {
            var bookingReview = new Booking_Review
            {
                Review_Status = "Posted",
                Review_Rating = bvm.Review_Rating,
                Review_Description = bvm.Review_Description,
                Date_Created = DateTime.Now,
                ClientId = bvm.ClientId,
                RoomId = bvm.RoomId,
            };

            try
            {
                _repository.Add(bookingReview);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(bookingReview);
        }

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
            return (_context.Booking_Reviews?.Any(e => e.BookingReviewId == id)).GetValueOrDefault();
        }
    }
}
