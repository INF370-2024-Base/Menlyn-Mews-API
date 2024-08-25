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
using Menlyn_Mews_API.Services;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VATsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repositroy;
        private readonly IAuditLogService _auditLogService; // Audit log service

        public VATsController(AppDbContext context, IRepositroy repositroy, IAuditLogService auditLogService)
        {
            _context = context;
            _repositroy = repositroy;
            _auditLogService = auditLogService; // Initialize audit log service
        }

        // GET: api/VATs
        [HttpGet]
        [Route("GetVAT")]
        public async Task<ActionResult> GetVAT()
        {
            try
            {
                var vat = await _repositroy.GetVATAsync();
                await _auditLogService.LogAsync("GET", "VAT", "GetVAT", "Retrieved VAT records"); // Log the action
                return Ok(vat);
            }
            catch (Exception ex)
            {
                await _auditLogService.LogAsync("GET", "VAT", "GetVAT", $"Error retrieving VAT records: {ex.Message}"); // Log the error
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
                if (vat == null)
                {
                    await _auditLogService.LogAsync("GET", "VAT", $"GetVATById {vatId}", "VAT not found"); // Log if not found
                    return NotFound("VAT Cannot Be Found");
                }

                await _auditLogService.LogAsync("GET", "VAT", $"GetVATById {vatId}", $"Retrieved VAT with ID {vatId}"); // Log the action
                return Ok(vat);
            }
            catch (Exception ex)
            {
                await _auditLogService.LogAsync("GET", "VAT", $"GetVATById {vatId}", $"Error retrieving VAT: {ex.Message}"); // Log the error
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
                if (vat == null)
                {
                    await _auditLogService.LogAsync("PUT", "VAT", $"UpdateVAT {vatId}", "VAT not found"); // Log if not found
                    return NotFound("VAT Cannot Be Found");
                }

                vat.VAT_Amount = vvm.VAT_Amount;
                vat.Last_Updated = DateTime.UtcNow;

                if (await _repositroy.SaveChangesAsync())
                {
                    await _auditLogService.LogAsync("PUT", "VAT", $"UpdateVAT {vatId}", $"Updated VAT with ID {vatId}"); // Log the update
                    return Ok(vat);
                }
            }
            catch (Exception ex)
            {
                await _auditLogService.LogAsync("PUT", "VAT", $"UpdateVAT {vatId}", $"Error updating VAT: {ex.Message}"); // Log the error
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
