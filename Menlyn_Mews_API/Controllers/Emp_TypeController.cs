using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Emp_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Emp_TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Emp_Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emp_Type>>> GetEmp_Types()
        {
            return await _context.Emp_Types.ToListAsync();
        }

        // GET: api/Emp_Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Emp_Type>> GetEmp_Type(int id)
        {
            var empType = await _context.Emp_Types.FindAsync(id);

            if (empType == null)
            {
                return NotFound();
            }

            return empType;
        }

        // POST: api/Emp_Type
        [HttpPost]
        public async Task<ActionResult<Emp_Type>> PostEmp_Type(Emp_Type empType)
        {
            _context.Emp_Types.Add(empType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmp_Type), new { id = empType.Id }, empType);
        }

        // PUT: api/Emp_Type/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmp_Type(int id, Emp_Type empType)
        {
            if (id != empType.Id)
            {
                return BadRequest();
            }

            _context.Entry(empType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Emp_TypeExists(id))
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

        // DELETE: api/Emp_Type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmp_Type(int id)
        {
            var empType = await _context.Emp_Types.FindAsync(id);
            if (empType == null)
            {
                return NotFound();
            }

            _context.Emp_Types.Remove(empType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Emp_TypeExists(int id)
        {
            return _context.Emp_Types.Any(e => e.Id == id);
        }
    }
}
