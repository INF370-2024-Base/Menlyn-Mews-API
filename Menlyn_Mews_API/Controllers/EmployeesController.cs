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
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public EmployeesController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                var results = await _repository.GetEmployeesAsync();

                dynamic employees = results.Select(e => new
                {
                    e.EmployeeId,
                    e.Employee_Name,
                    e.Employee_Surname,
                    e.Employee_ID_Number,
                    e.Employee_Email_Address,
                    e.Employee_Contact_Number,
                    e.Employee_Gender,
                    e.Employee_Address,
                    e.Employee_Photo,
                    Employee_Type = e.Employee_Type.Type_Description,
                    Position = e.Position.Position_Description,
                });

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetEmployeeById/{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployee(int employeeId)
        {
            try
            {
                var e = await _repository.GetEmployeeByIdAsync(employeeId);
                if (e == null) return NotFound("Employee Does Not Exist");

                dynamic employees = new
                {
                    e.EmployeeId,
                    e.Employee_Name,
                    e.Employee_Surname,
                    e.Employee_ID_Number,
                    e.Employee_Email_Address,
                    e.Employee_Contact_Number,
                    e.Employee_Gender,
                    e.Employee_Address,
                    e.Employee_Photo,
                    Employee_Type = e.Employee_Type.Type_Description,
                    Position = e.Position.Position_Description,
                };

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromForm] IFormCollection formData)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();

                if (!formData.ContainsKey("employee_id"))
                {
                    return BadRequest("Employee ID is required.");
                }

                int employeeId = Convert.ToInt32(formData["employee_id"]);
                var employee = await _context.Employees.FindAsync(employeeId);

                if (employee == null)
                {
                    return NotFound($"Employee with ID {employeeId} not found.");
                }

                // Check if a new file is uploaded
                if (formCollection.Files.Count > 0)
                {
                    var file = formCollection.Files.First();
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            string base64 = Convert.ToBase64String(fileBytes);

                            employee.Employee_Photo = base64;
                        }
                    }
                }

                // Update other fields
                employee.Employee_Name = formData["employee_name"];
                employee.Employee_Surname = formData["employee_surname"];
                employee.Employee_ID_Number = formData["employee_id_number"];
                employee.Employee_Email_Address = formData["employee_email_address"];
                employee.Employee_Contact_Number = formData["employee_contact_number"];
                employee.Employee_Gender = formData["employee_gender"];
                employee.Employee_Address = formData["employee_address"];
                employee.EmployeeTypeId = Convert.ToInt32(formData["employee_type"]);
                employee.PositionId = Convert.ToInt32(formData["employee_type"]);

                // Save changes to the database
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromForm] IFormCollection formData)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();

                var file = formCollection.Files.First();

                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();   
                        string base64 = Convert.ToBase64String(fileBytes);

                        var employee = new Employee
                        {
                            Employee_Name = formData["employee_name"],
                            Employee_Surname = formData["employee_surname"],
                            Employee_ID_Number = formData["employee_id_number"],
                            Employee_Email_Address = formData["employee_email_address"],
                            Employee_Contact_Number = formData["employee_contact_number"],
                            Employee_Gender = formData["employee_gender"],
                            Employee_Address = formData["employee_address"],
                            Employee_Photo = base64,
                            EmployeeTypeId = Convert.ToInt32(formData["employee_type"]),
                            PositionId = Convert.ToInt32(formData["employee_type"]),

                        };

                        _context.Employees.Add(employee);
                        await _context.SaveChangesAsync();
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
