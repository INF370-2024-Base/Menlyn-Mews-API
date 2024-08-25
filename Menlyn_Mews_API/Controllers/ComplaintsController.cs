using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Client;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public ComplaintsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetComplaints")]
        public async Task<ActionResult> GetComplaints()
        {
            try
            {
                var results = await _repository.GetComplaintsAsync();

                dynamic complaints = results.Select(c => new
                {
                    c.ComplaintId,
                    c.Complaint_Description,
                    c.Complaint_Date,
                    c.Complaint_Status,
                    Client = c.Client.Client_Name + " " + c.Client.Client_Surname,
                    Employee = c.Employee != null ? c.Employee?.Employee_Name + " " + c.Employee?.Employee_Surname : "Not Yet Resolved",
                    Complaint_Type = c.Complaint_Type.Complaint_Type_Description,
                });

                return Ok(complaints);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetComplaintById/{complaintId}")]
        public async Task<ActionResult<Complaint>> GetComplaint(int complaintId)
        {
            try
            {
                var c = await _repository.GetComplaintByIdAsync(complaintId);
                if (c == null) return NotFound("Complaint Does Not Exist");

                dynamic complaints = new
                {
                    c.ComplaintId,
                    c.Complaint_Description,
                    c.Complaint_Date,
                    c.Complaint_Status,
                    Client = c.Client.Client_Name + " " + c.Client.Client_Surname,
                    Employee = c.Employee != null ? c.Employee?.Employee_Name + " " + c.Employee?.Employee_Surname : "Not Yet Resolved",
                    Complaint_Type = c.Complaint_Type.Complaint_Type_Description,
                };

                return Ok(complaints);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddComplaint")]
        public async Task<IActionResult> PostComplaint(ComplaintViewModel cvm)
        {
            var complaint = new Complaint
            {
                Complaint_Description = cvm.Complaint_Description,
                Complaint_Date = DateTime.Now,
                Complaint_Status = "Not Resolved",
                ClientId = cvm.ClientId,
                EmployeeId = cvm.EmployeeId,
                ComplaintTypeId = cvm.ComplaintTypeId,
            };

            try
            {
                _repository.Add(complaint);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(complaint);
        }

        [HttpPut]
        [Route("ResolveComplaint/{complaintId}")]
        public async Task<ActionResult<ComplaintViewModel>> ResolveComplaint(int complaintId, ComplaintViewModel cvm)
        {
            try
            {
                var c = await _repository.GetComplaintByIdAsync(complaintId);
                if (c == null) return NotFound("Complaint Does Not Exist");

                c.EmployeeId = cvm.EmployeeId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(c);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        } 

        // DELETE: api/Complaints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            if (_context.Complaints == null)
            {
                return NotFound();
            }
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComplaintExists(int id)
        {
            return (_context.Complaints?.Any(e => e.ComplaintId == id)).GetValueOrDefault();
        }
    }
}
