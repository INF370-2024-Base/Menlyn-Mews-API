using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Emp_SheeftController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Emp_SheeftController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Emp_Sheeft
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emp_Sheeft>>> GetEmp_Sheefts()
        {
            return await _context.Emp_Sheefts.ToListAsync();
        }

        // GET: api/Emp_Sheeft/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Emp_Sheeft>> GetEmp_Sheeft(int id)
        {
            var empSheeft = await _context.Emp_Sheefts.FindAsync(id);

            if (empSheeft == null)
            {
                return NotFound();
            }

            return empSheeft;
        }

        // POST: api/Emp_Sheeft
        [HttpPost]
        public async Task<ActionResult<Emp_Sheeft>> PostEmp_Sheeft(Emp_Sheeft empSheeft)
        {
            _context.Emp_Sheefts.Add(empSheeft);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmp_Sheeft), new { id = empSheeft.Id }, empSheeft);
        }

        // PUT: api/Emp_Sheeft/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmp_Sheeft(int id, Emp_Sheeft empSheeft)
        {
            if (id != empSheeft.Id)
            {
                return BadRequest();
            }

            _context.Entry(empSheeft).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Emp_SheeftExists(id))
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

        // DELETE: api/Emp_Sheeft/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmp_Sheeft(int id)
        {
            var empSheeft = await _context.Emp_Sheefts.FindAsync(id);
            if (empSheeft == null)
            {
                return NotFound();
            }

            _context.Emp_Sheefts.Remove(empSheeft);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Emp_SheeftExists(int id)
        {
            return _context.Emp_Sheefts.Any(e => e.Id == id);
        }
    }
}
