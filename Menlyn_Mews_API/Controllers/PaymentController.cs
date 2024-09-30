using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repositroy;

        public PaymentController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repositroy = repositroy;
        }

        [HttpGet]
        [Route("GetPayments")]
        public async Task<ActionResult> GetAllPayments()
        {
            try
            {
                var results = await _repositroy.GetPaymentsAsync();

                dynamic payments = results.Select(p => new
                {
                    p.PaymentId,
                    p.Payment_Date.Date,
                    p.Payment_Status,
                    p.Payment_Amount,
                    p.Client.Client_Name,
                    p.Client.Client_Surname,
                    p.Payment_Type.Payment_Type_description,
                });

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetPaymentById/{paymentId}")]
        public async Task<ActionResult> GetPaymentById(int paymentId)
        {
            try
            {
                var p = await _repositroy.GetPaymentByIdAsync(paymentId);
                if (p == null) return NotFound("Payment Does Not Exist");


                dynamic payments = new
                {
                    p.PaymentId,
                    p.Payment_Date,
                    p.Payment_Status,
                    p.Payment_Amount,
                    p.Payment_Type.Payment_Type_description,
                };

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("RecordPayment")]
        public async Task<IActionResult> CreatePayment(PaymentViewModel pvm)
        {
            var payment = new Payment
            {
                Payment_Date = DateTime.Now,
                Payment_Amount = pvm.Payment_Amount,
                Payment_Status = "Approved",
                PaymentTypeId = pvm.PaymentTypeId,
                ClientId = pvm.ClientId,
            };

            try
            {
                _repositroy.Add(payment);
                await _repositroy.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }
            return Ok(payment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest("Payment ID mismatch.");
            }

            if (payment == null)
            {
                return BadRequest("Payment is null.");
            }

            try
            {
                _context.Entry(payment).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Payment.Any(e => e.PaymentId == id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var payment = await _context.Payment.FindAsync(id);
                if (payment == null)
                {
                    return NotFound();
                }

                _context.Payment.Remove(payment);
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

