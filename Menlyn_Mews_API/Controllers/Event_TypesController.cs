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
using Menlyn_Mews_API.ViewModels.Event;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Event_TypesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Event_TypesController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetEventTypes")]
        public async Task<ActionResult> GetEvent_Types()
        {
            try
            {
                var eventTypes = await _repository.GetEventTypesAsync();
                return Ok(eventTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEventTypeById/{eventTypeId}")]
        public async Task<ActionResult> GetEvent_Types(int eventTypeId)
        {
            try
            {
                var eventTypes = await _repository.GetEventTypesByIdAsync(eventTypeId);
                if (eventTypes == null) return NotFound("Event Type Does Not Exist");
                return Ok(eventTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEventType")]
        public async Task<ActionResult<EventTypeViewModel>> PutEvent_Types(int eventTypeId, EventTypeViewModel evm)
        {
            try
            {
                var eventTypes = await _repository.GetEventTypesByIdAsync(eventTypeId);
                if (eventTypes == null) return NotFound("Event Type Does Not Exist");

                eventTypes.Event_Description = evm.Event_Description;
                eventTypes.Event_Capacity_Status = evm.Event_Capacity_Status;
                eventTypes.Event_Type_Name = evm.Event_Type_Name;
                eventTypes.Event_Type_Price = evm.Event_Type_Price;
                eventTypes.Event_Capacity = evm.Event_Capacity;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(eventTypes);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddEventType")]
        public async Task<IActionResult> PostEvent_Types(EventTypeViewModel evm)
        {
            var eventType = new Event_Types
            {
                Event_Description = evm.Event_Description,
                Event_Capacity_Status= evm.Event_Capacity_Status,
                Event_Type_Name= evm.Event_Type_Name,
                Event_Type_Price= evm.Event_Type_Price,
                Event_Capacity = evm.Event_Capacity

            };

            try
            {
                _repository.Add(eventType);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(eventType);
        }

        // DELETE: api/Event_Types/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent_Types(int id)
        {
            if (_context.Event_Types == null)
            {
                return NotFound();
            }
            var event_Types = await _context.Event_Types.FindAsync(id);
            if (event_Types == null)
            {
                return NotFound();
            }

            _context.Event_Types.Remove(event_Types);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Event_TypesExists(int id)
        {
            return (_context.Event_Types?.Any(e => e.EventTypeId == id)).GetValueOrDefault();
        }
    }
}
