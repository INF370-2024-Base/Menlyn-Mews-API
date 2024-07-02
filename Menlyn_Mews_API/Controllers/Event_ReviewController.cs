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
    public class Event_ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Event_ReviewController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetEventReviews")]
        public async Task<ActionResult> GetEvent_Reviews()
        {
            try
            {
                var results = await _repository.GetEventReviewsAsync();

                dynamic eventReviews = results.Select(er => new
                {
                    er.EventReviewId,
                    er.Event_Review_Status,
                    er.Event_Review_Rating,
                    er.Event_Review_Description,
                    Client = er.Client.Client_Name + " " + er.Client.Client_Surname,
                });

                return Ok(eventReviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEventRevieById/{eventReviewId}")]
        public async Task<ActionResult<Event_Review>> GetEvent_Review(int eventReviewId)
        {
            try
            {
                var er = await _repository.GetEventReviewByIdAsync(eventReviewId);
                if (er == null) return NotFound("Event Review Does Not Exist");

                dynamic eventReviews = new
                {
                    er.EventReviewId,
                    er.Event_Review_Status,
                    er.Event_Review_Rating,
                    er.Event_Review_Description,
                    Client = er.Client.Client_Name + " " + er.Client.Client_Surname,
                };

                return Ok(eventReviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEventReview/{eventReviewId}")]
        public async Task<ActionResult<EventReviewViewModel>> PutEvent_Review(int eventReviewId, EventReviewViewModel evm)
        {
            try
            {
                var er = await _repository.GetEventReviewByIdAsync(eventReviewId);
                if (er == null) return NotFound("Event Review Does Not Exist");

                er.Event_Review_Status = evm.Event_Review_Status;
                er.Event_Review_Rating = evm.Event_Review_Rating;   
                er.Event_Review_Description = evm.Event_Review_Description;
                er.ClientId = evm.ClientId; 

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(er);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddEventReview")]
        public async Task<IActionResult> PostEvent_Review(EventReviewViewModel evm)
        {
            var eventReview = new Event_Review
            {
                Event_Review_Status = evm.Event_Review_Status,
                Event_Review_Rating = evm.Event_Review_Rating,
                Event_Review_Description = evm.Event_Review_Description,
                ClientId = evm.ClientId,
            };

            try
            {
                _repository.Add(eventReview);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(eventReview);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent_Review(int id)
        {
            if (_context.Event_Reviews == null)
            {
                return NotFound();
            }
            var event_Review = await _context.Event_Reviews.FindAsync(id);
            if (event_Review == null)
            {
                return NotFound();
            }

            _context.Event_Reviews.Remove(event_Review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Event_ReviewExists(int id)
        {
            return (_context.Event_Reviews?.Any(e => e.EventReviewId == id)).GetValueOrDefault();
        }
    }
}
