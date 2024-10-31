using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Employee;
using Menlyn_Mews_API.Services; // Include this for IAuditLogService

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        private readonly IAuditLogService _auditLogService;

        public ShiftsController(AppDbContext context, IRepositroy repository, IAuditLogService auditLogService)
        {
            _context = context;
            _repository = repository;
            _auditLogService = auditLogService; // Inject IAuditLogService
        }

        // GET: api/Shifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
        {
            try
            {
                var shifts = await _repository.GetShiftsAsync();
                var result = shifts.Select(s => new
                {
                    s.ShiftId,
                    Shift_Date = s.Shift_Date,
                    Start_Time = s.Start_TIme!.Value.ToString("hh:mm tt"),
                    End_Time = s.End_TIme!.Value.ToString("hh:mm tt"),
                    s.IP_Address,
                });

                // Log the action
                await _auditLogService.LogAsync("View", "ShiftsController", nameof(GetShifts), "Retrieved all shifts");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShiftById(int id)
        {
            try
            {
                var shift = await _repository.GetShiftByIdAsync(id);
                if (shift == null) return NotFound("Shift not found.");

                var result = new
                {
                    shift.ShiftId,
                    Shift_Date = shift.Shift_Date,
                    Start_Time = shift.Start_TIme!.Value.ToString("hh:mm tt"),
                    End_Time = shift.End_TIme!.Value.ToString("hh:mm tt"),
                    shift.IP_Address,
                };

                // Log the action
                await _auditLogService.LogAsync("View", "ShiftsController", nameof(GetShiftById), $"Retrieved shift with ID {id}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Shifts
        [HttpPost]
        public async Task<ActionResult<Shift>> CreateShift([FromBody] ShiftViewModel shiftViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shift = new Shift
            {
                Shift_Date = shiftViewModel.Shift_Date,
                Start_TIme = shiftViewModel.Start_TIme,
                End_TIme = shiftViewModel.End_TIme,
                IP_Address = GetUserIpAddress()
            };

            try
            {
                _repository.Add(shift);
                await _repository.SaveChangesAsync();

                // Log the action
                await _auditLogService.LogAsync("Create", "ShiftsController", nameof(CreateShift), $"Created shift with ID {shift.ShiftId}");

                return CreatedAtAction(nameof(GetShiftById), new { id = shift.ShiftId }, shift);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Shifts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShift(int id, [FromBody] ShiftViewModel shiftViewModel)
        {
            if (id == 0 || !ModelState.IsValid)
                return BadRequest(ModelState);

            var existingShift = await _repository.GetShiftByIdAsync(id);
            if (existingShift == null) return NotFound("Shift not found.");

            existingShift.Shift_Date = shiftViewModel.Shift_Date;
            existingShift.Start_TIme = shiftViewModel.Start_TIme;
            existingShift.End_TIme = shiftViewModel.End_TIme;
            existingShift.IP_Address = GetUserIpAddress();

            try
            {
                await _repository.SaveChangesAsync();

                // Log the action
                await _auditLogService.LogAsync("Update", "ShiftsController", nameof(UpdateShift), $"Updated shift with ID {id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            try
            {
                var shift = await _repository.GetShiftByIdAsync(id);
                if (shift == null)
                    return NotFound("Shift not found.");

                _repository.Delete(shift);
                await _repository.SaveChangesAsync();

                // Log the action
                await _auditLogService.LogAsync("Delete", "ShiftsController", nameof(DeleteShift), $"Deleted shift with ID {id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Helper method to retrieve IP Address
        private string GetUserIpAddress()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            if (remoteIpAddress != null)
            {
                return remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                    ? remoteIpAddress.ToString()
                    : remoteIpAddress.MapToIPv4().ToString();
            }
            return "IP Address not found";
        }
    }
}
