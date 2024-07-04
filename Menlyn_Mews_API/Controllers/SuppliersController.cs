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
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public SuppliersController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetSuppliers")]
        public async Task<ActionResult> GetSuppliers()
        {
            try
            {
                var results = await _repository.GetSuppliersAsync();

                dynamic suppliers = results.Select(s => new
                {
                    s.SupplierId,
                    s.Supplier_Name,
                    s.Supplier_Address_Line_1,
                    s.Supplier_Address_Line_2,
                    s.Supplier_Email,
                    s.Supplier_Contact_Number,
                    s.Supplier_Rep_Name,
                    s.Supplier_Rep_Surname,
                    s.Supplier_Rep_Email,
                    s.Supplier_Rep_Contact_Number,
                    s.City,
                    s.Province,
                    s.Postal_Code,
                    Supplier_Type = s.Supplier_Type.Supplier_Type_Description,
                });

                return Ok(suppliers); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSupplierById/{supplierId}")]
        public async Task<ActionResult> GetSupplier(int supplierId)
        {
            try
            {
                var s = await _repository.GetSupplierByIdAsync(supplierId);
                if (s == null) return NotFound("Supplier Does Not Exist");

                dynamic suppliers = new
                {
                    s.SupplierId,
                    s.Supplier_Name,
                    s.Supplier_Address_Line_1,
                    s.Supplier_Address_Line_2,
                    s.Supplier_Email,
                    s.Supplier_Contact_Number,
                    s.Supplier_Rep_Name,
                    s.Supplier_Rep_Surname,
                    s.Supplier_Rep_Email,
                    s.Supplier_Rep_Contact_Number,
                    s.City,
                    s.Province,
                    s.Postal_Code,
                    Supplier_Type = s.Supplier_Type.Supplier_Type_Description,
                };

                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSupplier/{supplierId}")]
        public async Task<ActionResult<SupplierViewModel>> PutSupplier(int supplierId, SupplierViewModel svm)
        {
            try
            {
                var s = await _repository.GetSupplierByIdAsync(supplierId);
                if (s == null) return NotFound("Supplier Does Not Exist");

                s.Supplier_Name = svm.Supplier_Name;
                s.Supplier_Address_Line_1 = svm.Supplier_Address_Line_1;
                s.Supplier_Address_Line_2 = svm.Supplier_Address_Line_2;
                s.Supplier_Email = svm.Supplier_Email;
                s.Supplier_Contact_Number = svm.Supplier_Contact_Number;
                s.Supplier_Rep_Name = svm.Supplier_Rep_Name;    
                s.Supplier_Rep_Surname = svm.Supplier_Rep_Surname;
                s.Supplier_Rep_Email = svm.Supplier_Rep_Email;  
                s.Supplier_Rep_Contact_Number = svm.Supplier_Rep_Contact_Number;
                s.City = svm.City;
                s.Province = svm.Province;
                s.Postal_Code = svm.Postal_Code;
                s.SupplierTypeId = svm.SupplierTypeId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(s);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddSupplier")]
        public async Task<IActionResult> PostSupplier(SupplierViewModel svm)
        {
            var supplier = new Supplier
            {
                Supplier_Name = svm.Supplier_Name,
                Supplier_Address_Line_1 = svm.Supplier_Address_Line_1,
                Supplier_Address_Line_2 = svm.Supplier_Address_Line_2,
                Supplier_Email = svm.Supplier_Email,
                Supplier_Contact_Number = svm.Supplier_Contact_Number,
                Supplier_Rep_Name = svm.Supplier_Rep_Name,
                Supplier_Rep_Surname = svm.Supplier_Rep_Surname,
                Supplier_Rep_Email = svm.Supplier_Rep_Email,
                Supplier_Rep_Contact_Number = svm.Supplier_Rep_Contact_Number,
                City = svm.City,
                Province = svm.Province,
                Postal_Code = svm.Postal_Code,
                SupplierTypeId = svm.SupplierTypeId,
            };

            try
            {
                _repository.Add(supplier);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }

            return Ok(supplier);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return (_context.Suppliers?.Any(e => e.SupplierId == id)).GetValueOrDefault();
        }
    }
}
