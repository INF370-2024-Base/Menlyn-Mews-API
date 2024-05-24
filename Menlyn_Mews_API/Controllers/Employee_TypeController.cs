using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employee_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Employee_TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee_Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee_Type>>> GetEmployee_Types()
        {
          if (_context.Employee_Types == null)
          {
              return NotFound();
          }
            return await _context.Employee_Types.ToListAsync();
        }

        // GET: api/Employee_Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee_Type>> GetEmployee_Type(int id)
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

            return employee_Type;
        }

        // PUT: api/Employee_Type/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee_Type(int id, Employee_Type employee_Type)
        {
            if (id != employee_Type.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee_Type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Employee_TypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employee_Type
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee_Type>> PostEmployee_Type(Employee_Type employee_Type)
        {
          if (_context.Employee_Types == null)
          {
              return Problem("Entity set 'AppDbContext.Employee_Types'  is null.");
          }
            _context.Employee_Types.Add(employee_Type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee_Type", new { id = employee_Type.Id }, employee_Type);
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
            return (_context.Employee_Types?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
