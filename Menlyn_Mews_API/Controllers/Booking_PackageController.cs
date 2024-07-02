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
    public class Booking_PackageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Booking_PackageController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetBookingPackages")]
        public async Task<ActionResult> GetBooking_Packages()
        {
            try
            {
                var bookingPackages = await _repository.GetBookingPackagesAsync();
                return Ok(bookingPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Booking_Package/5
        [HttpGet]
        [Route("GetBookingPackageById/{bookingPackageId}")]
        public async Task<ActionResult> GetBooking_Package(int bookingPackageId)
        {
            try
            {
                var bookingPackages = await _repository.GetBookingPackageByIdAsync(bookingPackageId);
                if (bookingPackages == null) return NotFound("Booking Package Does Not Exist");
                return Ok(bookingPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateBookingPackage/{bookingPackageId}")]
        public async Task<ActionResult<BookingPackageViewModel>> PutBooking_Package(int bookingPackageId, BookingPackageViewModel bvm)
        {
            try
            {
                var bookingPackages = await _repository.GetBookingPackageByIdAsync(bookingPackageId);
                if (bookingPackages == null) return NotFound("Booking Package Does Not Exist");

                bookingPackages.Booking_Package_Name = bvm.Booking_Package_Name;
                bookingPackages.Booking_Package_Description = bvm.Booking_Package_Description;
                bookingPackages.Booking_Package_Price = bvm.Booking_Package_Price;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(bookingPackages);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddBookingPackage")]
        public async Task<IActionResult> PostBooking_Package(BookingPackageViewModel bvm)
        {
            var bookingPackage = new Booking_Package
            {
                Booking_Package_Name = bvm.Booking_Package_Name,
                Booking_Package_Description = bvm.Booking_Package_Description,
                Booking_Package_Price = bvm.Booking_Package_Price,
            };

            try
            {
                _repository.Add(bookingPackage);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(bookingPackage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking_Package(int id)
        {
            if (_context.Booking_Packages == null)
            {
                return NotFound();
            }
            var booking_Package = await _context.Booking_Packages.FindAsync(id);
            if (booking_Package == null)
            {
                return NotFound();
            }

            _context.Booking_Packages.Remove(booking_Package);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Booking_PackageExists(int id)
        {
            return (_context.Booking_Packages?.Any(e => e.BookingPackageId == id)).GetValueOrDefault();
        }
    }
}
