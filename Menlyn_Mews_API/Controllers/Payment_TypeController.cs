

using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Payment_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Payment_TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Payment_Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment_Type>>> Get()
        {
            try
            {
                var paymentTypes = await _context.Payment_Types.ToListAsync();
                return Ok(paymentTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Payment_Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment_Type>> Get(int id)
        {
            try
            {
                var paymentType = await _context.Payment_Types.FindAsync(id);

                if (paymentType == null)
                {
                    return NotFound("Payment type not found.");
                }

                return Ok(paymentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Payment_Type
        [HttpPost]
        public async Task<ActionResult<Payment_Type>> Post([FromBody] Payment_Type paymentType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Payment_Types.Add(paymentType);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = paymentType.PaymentTypeId }, paymentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Payment_Type/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Payment_Type paymentType)
        {
            if (id != paymentType.PaymentTypeId)
            {
                return BadRequest("Payment Type ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(paymentType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Payment_Types.Any(e => e.PaymentTypeId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Payment_Type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var paymentType = await _context.Payment_Types.FindAsync(id);
                if (paymentType == null)
                {
                    return NotFound("Payment type not found.");
                }

                _context.Payment_Types.Remove(paymentType);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
