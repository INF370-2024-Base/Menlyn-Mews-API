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
using Menlyn_Mews_API.ViewModels.Supplier;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Supplier_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Supplier_TypeController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetSupplierTypes")]
        public async Task<ActionResult> GetSupplier_Types()
        {
            try
            {
                var supplierTypes = await _repository.GetSupplierTypesAsync();
                return Ok(supplierTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSupplierTypeById/{supplierTypeId}")]
        public async Task<ActionResult> GetSupplier_Type(int supplierTypeId)
        {
            try
            {
                var supplierTypes = await _repository.GetSupplierTypeByIdAsync(supplierTypeId);
                if (supplierTypes == null) return NotFound("Supplier Type Does Not Exist");
                return Ok(supplierTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSupplierType/{supplierTypeId}")]
        public async Task<ActionResult<SupplierTypeViewModel>> PutSupplier_Type(int supplierTypeId, SupplierTypeViewModel svm)
        {
            try
            {
                var supplierTypes = await _repository.GetSupplierTypeByIdAsync(supplierTypeId);
                if (supplierTypes == null) return NotFound("Supplier Type Does Not Exist");

                supplierTypes.Supplier_Type_Description = svm.Supplier_Type_Description;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(supplierTypes);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddSupplierType")]
        public async Task<IActionResult> PostSupplier_Type(SupplierTypeViewModel svm)
        {
            var supplierType = new Supplier_Type
            {
                Supplier_Type_Description = svm.Supplier_Type_Description,
            };

            try
            {
                _repository.Add(supplierType);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(supplierType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier_Type(int id)
        {
            if (_context.Supplier_Types == null)
            {
                return NotFound();
            }
            var supplier_Type = await _context.Supplier_Types.FindAsync(id);
            if (supplier_Type == null)
            {
                return NotFound();
            }

            _context.Supplier_Types.Remove(supplier_Type);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Supplier_TypeExists(int id)
        {
            return (_context.Supplier_Types?.Any(e => e.SupplierTypeId == id)).GetValueOrDefault();
        }
    }
}
