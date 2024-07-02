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
    public class Employee_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Employee_TypeController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetEmployeeType")]
        public async Task<ActionResult> GetEmployee_Types()
        {
            try
            {
                var employeetypes = await _repository.GetEmployeeTypesAsync();
                return Ok(employeetypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetEmployeeTypeById/{employeeTypeId}")]
        public async Task<ActionResult> GetEmployee_Type(int employeeTypeId)
        {
            try
            {
                var employeetypes = await _repository.GetEmployeeTypeByIdAsync(employeeTypeId);
                if (employeetypes == null) return NotFound("Employee Type Does Not Exist");
                return Ok(employeetypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEmployeeType/{employeeTypeId}")]
        public async Task<ActionResult<EmployeeTypeViewModel>> PutEmployee_Type(int employeeTypeId, EmployeeTypeViewModel evm)
        {
            try
            {
                var employeetypes = await _repository.GetEmployeeTypeByIdAsync(employeeTypeId);
                if (employeetypes == null) return NotFound("Employee Type Does Not Exist");

                employeetypes.Type_Description = evm.Type_Description;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(employeetypes);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpPost]
        [Route("AddEmployeeType")]
        public async Task<IActionResult> PostEmployee_Type(EmployeeTypeViewModel evm)
        {
            var employeetype = new Employee_Type
            {
                Type_Description = evm.Type_Description,
            };

            try
            {
                _repository.Add(employeetype);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(employeetype);
        }

        // DELETE: api/Employee_Type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee_Type(int id)
        {
            if (_context.Employee_Types == null)
            {
                return NotFound();
            }
            var employee_Type = await _context.Employee_Types.FindAsync(id);
            if (employee_Type == null)
            {
                return NotFound();
            }

            _context.Employee_Types.Remove(employee_Type);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Employee_TypeExists(int id)
        {
            return (_context.Employee_Types?.Any(e => e.EmployeeTypeId == id)).GetValueOrDefault();
        }
    }
}
