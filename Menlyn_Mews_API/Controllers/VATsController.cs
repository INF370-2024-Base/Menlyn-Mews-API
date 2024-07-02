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
using Menlyn_Mews_API.ViewModels.Floating_Tables;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VATsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repositroy;

        public VATsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repositroy = repositroy;
        }

        // GET: api/VATs
        [HttpGet]
        [Route("GetVAT")]
        public async Task<ActionResult> GetVAT()
        {
            try
            {
                var vat = await _repositroy.GetVATAsync();
                return Ok(vat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetVATById/{vatId}")]
        public async Task<ActionResult> GetVAT(int vatId)
        {
            try
            {
                var vat = await _repositroy.GetVATByIdAsync(vatId);
                if (vat == null) return NotFound("VAT Cannot Be Found");
                return Ok(vat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("UpdateVAT/{vatId}")]
        public async Task<ActionResult<VATViewModel>> PutVAT(int vatId, VATViewModel vvm)
        {
            try
            {
                var vat = await _repositroy.GetVATByIdAsync(vatId);
                if (vat == null) return NotFound("VAT Cannot Be Found");

                vat.VAT_Amount = vvm.VAT_Amount;
                vat.Last_Updated = DateTime.UtcNow;

                if (await _repositroy.SaveChangesAsync())
                {
                    return Ok(vat);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

    }
}
