using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Event;
using Menlyn_Mews_API.Models.Domain.Emails;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Event_BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        private readonly IGeneralEmailService _emailService;

        public Event_BookingController(AppDbContext context, IRepositroy repositroy, IGeneralEmailService generalEmailService)
        {
            _context = context;
            _repository = repositroy;   
            _emailService = generalEmailService;
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
                    eb.Event_Date,
                    eb.Event_Price,
                    eb.Start_Time,
                    eb.End_Time,
                    eb.Event_Status,
                    eb.Allergy_Description,
                    Event_Type = eb.Event_Types.Event_Description,
                    Client = eb.Client.Client_Name + " " + eb.Client.Client_Surname,
                    Employee = eb.Employee.Employee_Name + " " + eb.Employee.Employee_Surname,
                    eb.Date_Sent,
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
                    eb.Event_Date,
                    eb.Event_Price,
                    eb.Start_Time,
                    eb.End_Time,
                    eb.Event_Status,
                    eb.Allergy_Description,
                    eb.EventTypeId,
                    eb.ClientId,
                    eb.EmployeeId,
                    eb.Date_Sent,
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

        [HttpPut]
        [Route("PayEventBooking/{eventId}")]
        public async Task<ActionResult<EventBookingViewModel>> PayEventBooking(int eventId, EventBookingViewModel evm)
        {
            try
            {
                var eb = await _repository.GetEventBookingByIdAsync(eventId);
                if (eb == null) return NotFound("Event Booking Does Not Exist");

                eb.Event_Date = evm.Event_Date;
                eb.Event_Price = evm.Event_Price;

                if (eb.Date_Sent.HasValue && (DateTime.Now - eb.Date_Sent.Value).TotalDays > 3 && eb.Event_Status != "Confirmed")
                {
                    eb.Event_Status = "Cancelled";
                    return BadRequest("The event booking has been cancelled as 3 days have elapsed since it was sent.");
                }


                if (evm.Start_Time.HasValue)
                {
                    eb.Start_Time = evm.Start_Time.Value.TimeOfDay;
                }

                if (evm.End_Time.HasValue)
                {
                    eb.End_Time = evm.End_Time.Value.TimeOfDay;
                }

                eb.Event_Status = "Confirmed";
                eb.Allergy_Description = evm.Allergy_Description;
                eb.EventTypeId = evm.EventTypeId;
                eb.ClientId = evm.ClientId;
                eb.EmployeeId = evm.EmployeeId;

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
                Start_Time = evm.Start_Time.Value.TimeOfDay,
                End_Time = evm.End_Time.Value.TimeOfDay,
                Event_Status = "Pending",
                Allergy_Description = evm.Allergy_Description,
                EventTypeId = evm.EventTypeId,
                ClientId = evm.ClientId,
                EmployeeId = evm.EmployeeId,
                Date_Sent = DateTime.Now
            };

            var clientInfo = await _repository.GetClientByIdAsync(eventBooking.ClientId);
            if (clientInfo == null) 
            { 
                return NotFound("Client Does Not Exist");
            }

            var eventType = await _repository.GetEventTypesByIdAsync(eventBooking.EventTypeId);

            try
            {
                _repository.Add(eventBooking);
                await _repository.SaveChangesAsync();
                var eventIdBytes = BitConverter.GetBytes(eventBooking.EventId);
                var clientIdBytes = BitConverter.GetBytes(eventBooking.ClientId);

                var encodedEventId = Convert.ToBase64String(eventIdBytes);
                var encodedClientId = Convert.ToBase64String(clientIdBytes);
                var eventPaymentScreenLink = $"http://localhost:4200/event-payment/{encodedEventId}/{encodedClientId}";

                var mailrequest = new Mailrequest
                {
                    ToEmail = clientInfo.Client_Email_Address,
                    Subject = "Menlyn Mews Event Booking",
                    Body = GenerateEventReceiptEmailBody(eventPaymentScreenLink, clientInfo.Client_Name!,
                                                         evm.Event_Date.ToString("yyyy-MM-dd"),
                                                         evm.Start_Time.Value.ToString("hh:mm tt"),
                                                         evm.End_Time.Value.ToString("hh:mm tt"),
                                                         evm.Event_Price, eventType.Event_Description)
                };

                await _emailService.SendEmailAsync(mailrequest);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(eventBooking);
        }


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

        private string GenerateEventReceiptEmailBody(string link, string name, string eventDate, string startTime, string endTime, decimal price, string eventName)
        {
            var htmlContent = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    width: 100%;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    max-width: 600px;
                    margin: 50px auto;
                }}
                .header {{
                    background-color: #8B0000;
                    color: #ffffff;
                    padding: 20px;
                    text-align: center;
                    border-top-left-radius: 10px;
                    border-top-right-radius: 10px;
                }}
                .content {{
                    padding: 20px;
                    font-size: 16px;
                    line-height: 1.6;
                    text-align: left;
                }}
                .content p {{
                    margin-bottom: 20px;
                }}
                .button {{
                    display: inline-block;
                    background-color: #8B0000;
                    color: #ffffff;
                    padding: 10px 20px;
                    text-decoration: none;
                    border-radius: 5px;
                    font-size: 16px;
                }}
                .footer {{
                    padding: 20px;
                    text-align: center;
                    font-size: 14px;
                    color: #888888;
                }}
                .event-details {{
                    background-color: #f9f9f9;
                    padding: 15px;
                    border-radius: 5px;
                    margin-bottom: 20px;
                }}
                .event-details p {{
                    margin: 10px 0;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Your Event Payment Details</h1>
                </div>
                <div class='content'>
                    <p>Dear {name},</p>
                    <p>Thank you for booking your event with Menlyn Mews. Below are the details of your upcoming event:</p>

                    <div class='event-details'>
                        <p><strong>Event:</strong> {eventName}</p>
                        <p><strong>Event Date:</strong> {eventDate}</p>
                        <p><strong>Start Time:</strong> {startTime}</p>
                        <p><strong>End Time:</strong> {endTime}</p>
                        <p><strong>Total Price:</strong> R{price}</p>
                    </div>

                    <p>To confirm and make payment for your event, please click the button below:</p>
                    <p style='text-align: center;'>
                        <a href='{link}' class='button'>Proceed to Payment</a>
                    </p>

                    <p>If you have any questions, feel free to reach out to us. We look forward to hosting your event! <strong>If payment is not received within 3 days of this email, your booking will be lost</strong></p>
                </div>
                <div class='footer'>
                    <p>Thank you, <br/>Menlyn Mews Team</p>
                </div>
            </div>
        </body>
        </html>";

            return htmlContent;
        }

    }
}
