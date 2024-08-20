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
using Menlyn_Mews_API.ViewModels.Booking;
using System.Security.Cryptography;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public DiscountsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("CheckCode")]
        public async Task<ActionResult> FindDiscount(string code)
        {
            try
            {
                var discount = await _repository.FindDiscountCodeAsync(code);
                if (discount == null) return NotFound("Code Does Not Exist!");

                return Ok( new 
                { 
                    discountId = discount.DiscountId,
                    amount = discount.Discount_Percenatage,
                });
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("GetDiscounts")]
        public async Task<ActionResult> GetDiscount()
        {
            try
            {
                var discounts = await _repository.GetDiscountsAsync();
                return Ok(discounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDiscountById/{discountId}")]
        public async Task<ActionResult<Discount>> GetDiscount(int discountId)
        {
            try
            {
                var discounts = await _repository.GetDiscountByIdAsync(discountId);
                if (discounts == null) return NotFound("Discount Code Does Not Exist");
                return Ok(discounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateDiscount/{discountId}")]
        public async Task<ActionResult<DiscountViewModel>> PutDiscount(int discountId, DiscountViewModel dvm)
        {
            try
            {
                var discounts = await _repository.GetDiscountByIdAsync(discountId);
                if (discounts == null) return NotFound("Discount Code Does Not Exist");

                discounts.Discount_Name = dvm.Discount_Name;
                discounts.Discount_Code = dvm.Discount_Code;
                discounts.Discount_Percenatage = dvm.Discount_Percenatage;
                discounts.Start_Date = dvm.Start_Date;
                discounts.End_Date = dvm.End_Date;
                discounts.Is_Active = dvm.Is_Active;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(discounts);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("Add5DiscountCode/{discountAmount}")]
        public async Task<IActionResult> Post5Discount(decimal discountAmount, DiscountViewModel dvm)
        {
            var discountCode = GenerateDiscountCode();

            var existingDiscount = await _context.Discount.FirstOrDefaultAsync(d => d.Discount_Code == discountCode);

            if (existingDiscount != null) 
            {
                return BadRequest("Discount Code Already Exists, Please Click Generate Again!");
            }

            var discount = new Discount
            {
                Discount_Name = dvm.Discount_Name,
                Discount_Code = discountCode,  
                Discount_Percenatage = discountAmount,
                Start_Date = dvm.Start_Date,
                End_Date = dvm.End_Date,
                Is_Active = dvm.Is_Active,
            };

            try
            {
                _repository.Add(discount);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(discount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            if (_context.Discount == null)
            {
                return NotFound();
            }
            var discount = await _context.Discount.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            _context.Discount.Remove(discount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string GenerateDiscountCode()
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "1234567890";
            string discountCode = "";

            Random num = new Random();

            discountCode = num.Next(10, 100).ToString();

            for (int i = 0; i <= 5; i++)
            {
                discountCode += letters[num.Next(0, 26)];

            }

            for (int i = 0; i < 3; i++)
            {
                discountCode += numbers[num.Next(0, 10)];
            }

            return discountCode;
        }
    }
}
