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
using Menlyn_Mews_API.ViewModels.Employee;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employee_ShiftController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Employee_ShiftController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetEmployeeShifts")]
        public async Task<ActionResult> GetEmployee_Shifts()
        {
            try
            {
                var results = await _repository.GetEmployeeShiftsAsync();

                dynamic employeeshifts = results.Select(es => new
                {
                    es.Employee.EmployeeId,
                    es.Shift.ShiftId,
                    Employee_Name = es.Employee.Employee_Name,
                    Shift_Time = es.Shift.Start_TIme + " - " + es.Shift.End_TIme,
                    Shift_Date = es.Shift.Shift_Date,
                    es.Clock_In_Time,
                    es.Clock_Out_Time,
                    es.Shift_Description,
                });

                return Ok(employeeshifts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeShiftById/{employeeId}")]
        public async Task<ActionResult> GetEmployee_Shift(int employeeId, int shiftId)
        {
            try
            {
                var es = await _repository.GetEmployeeShiftByIdAsync(employeeId, shiftId);
                if (es == null) return NotFound("Employee Shift Does Not Exist");

                dynamic employeeshifts = new
                {
                    es.Employee.EmployeeId,
                    es.Shift.ShiftId,
                    Employee_Name = es.Employee.Employee_Name,
                    Shift_Time = es.Shift.Start_TIme + " - " + es.Shift.End_TIme,
                    Shift_Date = es.Shift.Shift_Date,
                    es.Clock_In_Time,
                    es.Clock_Out_Time,
                    es.Shift_Description,
                };

                return Ok(employeeshifts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEmployeeShift/{employeeId}")]
        public async Task<ActionResult<EmployeeShiftViewModel>> PutEmployee_Shift(int employeeId, int shiftId, EmployeeShiftViewModel esvm)
        {
            try
            {
                var employeeShift = await _repository.GetEmployeeShiftByIdAsync(employeeId, shiftId);
                if (employeeShift == null) return NotFound("Employee Shift Not Found");

                employeeShift.Clock_In_Time = esvm.Clock_In_Time;
                employeeShift.Clock_Out_Time = esvm.Clock_Out_Time;
                employeeShift.Shift_Description = esvm.Shift_Description;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(employeeShift);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Employee_Shift
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee_Shift>> PostEmployee_Shift(Employee_Shift employee_Shift)
        {
          if (_context.Employee_Shifts == null)
          {
              return Problem("Entity set 'AppDbContext.Employee_Shifts'  is null.");
          }
            _context.Employee_Shifts.Add(employee_Shift);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Employee_ShiftExists(employee_Shift.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee_Shift", new { id = employee_Shift.EmployeeId }, employee_Shift);
        }

        // DELETE: api/Employee_Shift/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee_Shift(int id)
        {
            if (_context.Employee_Shifts == null)
            {
                return NotFound();
            }
            var employee_Shift = await _context.Employee_Shifts.FindAsync(id);
            if (employee_Shift == null)
            {
                return NotFound();
            }

            _context.Employee_Shifts.Remove(employee_Shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Employee_ShiftExists(int id)
        {
            return (_context.Employee_Shifts?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
