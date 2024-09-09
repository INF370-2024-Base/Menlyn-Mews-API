using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Emp
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emp>>> GetEmps()
        {
            return await _context.Emps.ToListAsync();
        }

        // GET: api/Emp/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Emp>> GetEmp(int id)
        {
            var emp = await _context.Emps.FindAsync(id);

            if (emp == null)
            {
                return NotFound();
            }

            return emp;
        }

        // POST: api/Emp
        [HttpPost]
        public async Task<ActionResult<Emp>> PostEmp(Emp emp)
        {
            _context.Emps.Add(emp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmp), new { id = emp.Id }, emp);
        }

        // PUT: api/Emp/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmp(int id, Emp emp)
        {
            if (id != emp.Id)
            {
                return BadRequest();
            }

            _context.Entry(emp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpExists(id))
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

        // DELETE: api/Emp/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmp(int id)
        {
            var emp = await _context.Emps.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            _context.Emps.Remove(emp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpExists(int id)
        {
            return _context.Emps.Any(e => e.Id == id);
        }
    }
}
