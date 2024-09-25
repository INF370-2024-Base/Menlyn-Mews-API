using Microsoft.AspNetCore.Mvc;
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
                    UserEmail = e.ApplicationUser.Email,
                    UserName = e.ApplicationUser.UserName,
                });

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet("{employeeId}")]
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
                    e.EmployeeTypeId,
                    e.PositionId,
                    e.RateId
                };

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("EmployeeDetails/{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int employeeId)
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
                    e.Employee_Type.Type_Description,
                    e.Position.Position_Description,
                    e.Rates.Rate,
                    e.ApplicationUser.UserName,
                    e.ApplicationUser.Email,
                };

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, EmployeeViewModel evm)
        {
            try
            {
                var employee = await _repository.GetEmployeeByIdAsync(employeeId);
                if (employee == null) return NotFound("Employee Does Not Exist");

                employee.Employee_Name = evm.Employee_Name;
                employee.Employee_Surname = evm.Employee_Surname;
                employee.Employee_ID_Number = evm.Employee_ID_Number;
                employee.Employee_Email_Address = evm.Employee_Email_Address;
                employee.Employee_Contact_Number = evm.Employee_Contact_Number;
                employee.Employee_Gender = evm.Employee_Gender;
                employee.Employee_Address = evm.Employee_Address;
                employee.EmployeeTypeId = evm.EmployeeTypeId;
                employee.PositionId = evm.PositionId;
                employee.RateId = evm.RateId;

                if (!string.IsNullOrEmpty(evm.Employee_Photo))
                {
                    employee.Employee_Photo = evm.Employee_Photo;
                }

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(employee);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }


        //[HttpPost, DisableRequestSizeLimit]
        //public async Task<IActionResult> AddEmployee([FromForm] IFormCollection formData)
        //{
        //    try
        //    {
        //        var formCollection = await Request.ReadFormAsync();

        //        var file = formCollection.Files.First();

        //        if (file.Length > 0)
        //        {
        //            using (var ms = new MemoryStream())
        //            {
        //                file.CopyTo(ms);
        //                var fileBytes = ms.ToArray();   
        //                string base64 = Convert.ToBase64String(fileBytes);

        //                var employee = new Employee
        //                {
        //                    Employee_Name = formData["employee_name"],
        //                    Employee_Surname = formData["employee_surname"],
        //                    Employee_ID_Number = formData["employee_id_number"],
        //                    Employee_Email_Address = formData["employee_email_address"],
        //                    Employee_Contact_Number = formData["employee_contact_number"],
        //                    Employee_Gender = formData["employee_gender"],
        //                    Employee_Address = formData["employee_address"],
        //                    Employee_Photo = base64,
        //                    EmployeeTypeId = Convert.ToInt32(formData["employee_type"]),
        //                    PositionId = Convert.ToInt32(formData["employee_type"]),

        //                };

        //                _context.Employees.Add(employee);
        //                await _context.SaveChangesAsync();
        //            }

        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}

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
