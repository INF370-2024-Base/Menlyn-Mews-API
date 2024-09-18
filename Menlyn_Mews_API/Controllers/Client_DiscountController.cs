using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Client_DiscountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Client_DiscountController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetClientDiscounts")]
        public async Task<ActionResult> GetClient_Discounts()
        {
            try
            {
                var results = await _repository.GetClientDiscountsAsync();

                dynamic clientDiscounts = results.Select(cd => new
                {
                    cd.ClientId,
                    cd.DiscountId,
                });

                return Ok(clientDiscounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient_Discount(int id, Client_Discount client_Discount)
        {
            if (id != client_Discount.DiscountId)
            {
                return BadRequest();
            }

            _context.Entry(client_Discount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Client_DiscountExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Client_Discount>> PostClient_Discount(Client_Discount client_Discount)
        {
          if (_context.Client_Discounts == null)
          {
              return Problem("Entity set 'AppDbContext.Client_Discounts'  is null.");
          }
            _context.Client_Discounts.Add(client_Discount);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Client_DiscountExists(client_Discount.DiscountId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClient_Discount", new { id = client_Discount.DiscountId }, client_Discount);
        }

        // DELETE: api/Client_Discount/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient_Discount(int id)
        {
            if (_context.Client_Discounts == null)
            {
                return NotFound();
            }
            var client_Discount = await _context.Client_Discounts.FindAsync(id);
            if (client_Discount == null)
            {
                return NotFound();
            }

            _context.Client_Discounts.Remove(client_Discount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Client_DiscountExists(int id)
        {
            return (_context.Client_Discounts?.Any(e => e.DiscountId == id)).GetValueOrDefault();
        }
    }
}
