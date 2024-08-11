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
    public class RatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public RatesController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetRates")]
        public async Task<ActionResult> GetRates()
        {
            try
            {
                var rates = await _repository.GetRatesAsync();
                return Ok(rates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRateById/{rateId}")]
        public async Task<ActionResult> GetRates(int rateId)
        {
            try
            {
                var rate = await _repository.GetRatesByIdAsync(rateId);
                if (rate == null) return NotFound("Rate Does Not Exist");
                return Ok(rate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRate/{rateId}")]
        public async Task<ActionResult<RateViewModel>> PutRates(int rateId, RateViewModel rvm)
        {
            try
            {
                var rate = await _repository.GetRatesByIdAsync(rateId);
                if (rate == null) return NotFound("Rate Does Not Exist");

                rate.Employee_Type = rvm.Employee_Type;
                rate.Rate = rvm.Rate;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(rate);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpPost]
        [Route("CreateRate")]
        public async Task<IActionResult> PostRates(RateViewModel rvm)
        {
            var rate = new Rates
            {
                Employee_Type = rvm.Employee_Type,
                Rate = rvm.Rate
            };

            try
            {
                _repository.Add(rate);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(rate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRates(int id)
        {
            if (_context.Rates == null)
            {
                return NotFound();
            }
            var rates = await _context.Rates.FindAsync(id);
            if (rates == null)
            {
                return NotFound();
            }

            _context.Rates.Remove(rates);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
