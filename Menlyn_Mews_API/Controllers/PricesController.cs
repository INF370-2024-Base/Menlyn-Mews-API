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
using Menlyn_Mews_API.ViewModels.Products;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public PricesController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetPrices")]
        public async Task<ActionResult> GetPrices()
        {
            try
            {
                var prices = await _repository.GetPricesAsync();
                return Ok(prices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPriceById/{priceId}")]
        public async Task<ActionResult> GetPrice(int priceId)
        {
            try
            {
                var prices = await _repository.GetPriceByIdAsync(priceId);
                if (prices == null) return NotFound("Price Does Not Exist");
                return Ok(prices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdatePrice/{priceId}")]
        public async Task<ActionResult<PriceViewModel>> PutPrice(int priceId, PriceViewModel pvm)
        {
            try
            {
                var prices = await _repository.GetPriceByIdAsync(priceId);
                if (prices == null) return NotFound("Price Does Not Exist");
                
                prices.Product_Price = pvm.Product_Price;
                prices.Price_Date = DateTime.Now;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(prices);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddPrice")]
        public async Task<IActionResult> PostPrice(PriceViewModel pvm)
        {
            var price = new Price
            {
                Product_Price = pvm.Product_Price,
                Price_Date = DateTime.Now,
            };

            try
            {
                _repository.Add(price);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(price);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(int id)
        {
            if (_context.Prices == null)
            {
                return NotFound();
            }
            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }

            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PriceExists(int id)
        {
            return (_context.Prices?.Any(e => e.PriceId == id)).GetValueOrDefault();
        }
    }
}
