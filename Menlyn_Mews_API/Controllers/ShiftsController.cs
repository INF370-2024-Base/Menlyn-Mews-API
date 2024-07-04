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
using System.Net;
using Menlyn_Mews_API.ViewModels.Employee;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public ShiftsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetShifts")]
        public async Task<ActionResult> GetShifts()
        {
            try
            {
                var resutls = await _repository.GetShiftsAsync();

                dynamic shifts = resutls.Select(s => new
                {
                    s.ShiftId,
                    Shift_Date = s.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Start_Time = s.Start_TIme!.Value.ToString("hh:mm tt"),
                    End_Time = s.End_TIme!.Value.ToString("hh:mm tt"),
                    s.IP_Address,
                }); ;

                return Ok(shifts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetShiftById/{shiftId}")]
        public async Task<ActionResult> GetShift(int shiftId)
        {
            try
            {
                var s = await _repository.GetShiftByIdAsync(shiftId);
                if (s == null) return NotFound("Shift Does Not Exist");

                dynamic shifts = new
                {
                    s.ShiftId,
                    Shift_Date = s.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Start_Time = s.Start_TIme!.Value.ToString("hh:mm tt"),
                    End_Time = s.End_TIme!.Value.ToString("hh:mm tt"),
                    s.IP_Address,
                };

                return Ok(shifts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateShiftById/{shiftId}")]
        public async Task<ActionResult<ShiftViewModel>> PutShift(int shiftId, ShiftViewModel svm)
        {
            try
            {
                var s = await _repository.GetShiftByIdAsync(shiftId);
                if (s == null) return NotFound("Shift Does Not Exist");

                s.Shift_Date = svm.Shift_Date;
                s.Start_TIme = svm.Start_TIme; 
                s.End_TIme = svm.End_TIme;
                s.IP_Address = GetUserIpAddress();

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(s);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddShift")]
        public async Task<IActionResult> PostShift(ShiftViewModel svm)
        {
            var shift = new Shift
            {
                Shift_Date = svm.Shift_Date,
                Start_TIme = svm.Start_TIme,
                End_TIme = svm.End_TIme,
                IP_Address = GetUserIpAddress(),
            };

            try
            {
                _repository.Add(shift);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(shift);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            if (_context.Shifts == null)
            {
                return NotFound();
            }
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string GetUserIpAddress()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return remoteIpAddress.ToString();
                }

                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    var ipv4Address = remoteIpAddress.MapToIPv4().ToString();
                    return ipv4Address;
                }

                return remoteIpAddress.ToString();
            }
            return "IP Address not found";
        }
    }
}
