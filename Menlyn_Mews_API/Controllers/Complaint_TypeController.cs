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
using Menlyn_Mews_API.ViewModels.Client;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Complaint_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Complaint_TypeController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetComplaintTypes")]
        public async Task<ActionResult> GetComplaint_Types()
        {
            try
            {
                var complaintTypes = await _repository.GetComplaintTypesAsync();
                return Ok(complaintTypes);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetComplaintTypeById{complaintTypeId}")]
        public async Task<ActionResult> GetComplaint_Type(int complaintTypeId)
        {
            try
            {
                var complaintTypes = await _repository.GetComplaintTypeByIdAsync(complaintTypeId);
                if (complaintTypes == null) return NotFound("Complaint Type Not Found");
                return Ok(complaintTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateComplaintTypeId/{complaintTypeId}")]
        public async Task<ActionResult<ComplaintTypeViewModel>> PutComplaint_Type(int complaintTypeId, ComplaintTypeViewModel cvm)
        {
            try
            {
                var complaintTypes = await _repository.GetComplaintTypeByIdAsync(complaintTypeId);
                if (complaintTypes == null) return NotFound("Complaint Type Not Found");

                complaintTypes.Complaint_Type_Description = cvm.Complaint_Type_Description;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(complaintTypes);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddComplaintType")]
        public async Task<IActionResult> PostComplaint_Type(ComplaintTypeViewModel cvm)
        {
            var complaintType = new Complaint_Type
            {
                Complaint_Type_Description = cvm.Complaint_Type_Description,
            };

            try
            {
                _repository.Add(complaintType);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(complaintType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint_Type(int id)
        {
            if (_context.Complaint_Types == null)
            {
                return NotFound();
            }
            var complaint_Type = await _context.Complaint_Types.FindAsync(id);
            if (complaint_Type == null)
            {
                return NotFound();
            }

            _context.Complaint_Types.Remove(complaint_Type);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
