using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using NuGet.Packaging.Core;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Event;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Event_BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Event_BookingController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetEventBookings")]
        public async Task<ActionResult> GetEvent_Bookings()
        {
            try
            {
                var results = await _repository.GetEventBookingsAsync();

                dynamic eventBookings = results.Select(eb => new
                {
                    eb.EventId,
                    Event_Date = eb.Event_Date.Date.ToString("yyyy-MM-dd"),
                    eb.Event_Price,
                    eb.Start_Time,
                    eb.End_Time,
                    eb.Event_Status,
                    eb.Allergy_Description,
                    Event_Type = eb.Event_Types.Event_Description,
                    Client = eb.Client.Client_Name + " " + eb.Client.Client_Surname,
                    Employee = eb.Employee_Shift.Employee.Employee_Name + " " + eb.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date = eb.Employee_Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (eb.Employee_Shift.Shift.Start_TIme.HasValue && eb.Employee_Shift.Shift.End_TIme.HasValue)
                          ? eb.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + eb.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,
                });

                return Ok(eventBookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEventBookingById/{eventId}")]
        public async Task<ActionResult> GetEvent_Booking(int eventId)
        {
            try
            {
                var eb = await _repository.GetEventBookingByIdAsync(eventId);
                if (eb == null) return NotFound("Event Booking Does Not Exist");

                dynamic eventBookings = new
                {
                    eb.EventId,
                    Event_Date = eb.Event_Date.Date.ToString("yyyy-MM-dd"),
                    eb.Event_Price,
                    eb.Start_Time,
                    eb.End_Time,
                    eb.Event_Status,
                    eb.Allergy_Description,
                    Event_Type = eb.Event_Types.Event_Description,
                    Client = eb.Client.Client_Name + " " + eb.Client.Client_Surname,
                    Employee = eb.Employee_Shift.Employee.Employee_Name + " " + eb.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date = eb.Employee_Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (eb.Employee_Shift.Shift.Start_TIme.HasValue && eb.Employee_Shift.Shift.End_TIme.HasValue)
                          ? eb.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + eb.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,
                };

                return Ok(eventBookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEventBooking/{eventId}")]
        public async Task<ActionResult<EventBookingViewModel>> PutEvent_Booking(int eventId, EventBookingViewModel evm)
        {
            try
            {
                var eb = await _repository.GetEventBookingByIdAsync(eventId);
                if (eb == null) return NotFound("Event Booking Does Not Exist");

                eb.Event_Date = evm.Event_Date;
                eb.Event_Price = evm.Event_Price;

                if (evm.Start_Time.HasValue)
                {
                    eb.Start_Time = evm.Start_Time.Value.TimeOfDay;
                }

                if (evm.End_Time.HasValue)
                {
                    eb.End_Time = evm.End_Time.Value.TimeOfDay;
                }

                eb.Event_Status = evm.Event_Status;
                eb.Allergy_Description = evm.Allergy_Description;
                eb.EventTypeId = evm.EventTypeId;
                eb.ClientId = evm.ClientId;
                eb.EmployeeId = evm.EmployeeId;
                eb.ShiftId = evm.ShiftId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(eb);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("CreateEventBooking")]
        public async Task<IActionResult> PostEvent_Booking(EventBookingViewModel evm)
        {

            var eventBooking = new Event_Booking
            {
                Event_Date = evm.Event_Date,
                Event_Price = evm.Event_Price,
                Start_Time =  evm.Start_Time.Value.TimeOfDay,
                End_Time = evm.End_Time.Value.TimeOfDay,
                Event_Status = evm.Event_Status,
                Allergy_Description = evm.Allergy_Description,
                EventTypeId = evm.EventTypeId,
                ClientId = evm.ClientId,
                EmployeeId = evm.EmployeeId,
                ShiftId = evm.ShiftId,
            };

            try
            {
                _repository.Add(eventBooking);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(eventBooking);
        }

        //[HttpPost]
        //[Route("CreateEventBooking")]
        //public async Task<IActionResult> PostEvent_Booking([FromBody] EventBookingViewModel evm)
        //{
        //    if (evm == null)
        //    {
        //        return BadRequest("EventBookingViewModel is null.");
        //    }

        //    TimeSpan? startTime = evm.Start_Time?.TimeOfDay;
        //    TimeSpan? endTime = evm.End_Time?.TimeOfDay;

        //    var eventBooking = new Event_Booking
        //    {
        //        Event_Date = evm.Event_Date,
        //        Event_Price = evm.Event_Price,
        //        Start_Time = startTime,
        //        End_Time = endTime,
        //        Event_Status = evm.Event_Status,
        //        Allergy_Description = evm.Allergy_Description,
        //        EventTypeId = evm.EventTypeId,
        //        ClientId = evm.ClientId,
        //        EmployeeId = evm.EmployeeId,
        //        ShiftId = evm.ShiftId,
        //    };

        //    try
        //    {
        //        _repository.Add(eventBooking);
        //        await _repository.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    return Ok(eventBooking);
        //}


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent_Booking(int id)
        {
            if (_context.Event_Bookings == null)
            {
                return NotFound();
            }
            var event_Booking = await _context.Event_Bookings.FindAsync(id);
            if (event_Booking == null)
            {
                return NotFound();
            }

            _context.Event_Bookings.Remove(event_Booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Event_BookingExists(int id)
        {
            return (_context.Event_Bookings?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}
