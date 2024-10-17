using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For DbContext
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Client;
using System.Data.SqlClient; // For SQL connection

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        private readonly string _connectionString;

        public ComplaintsController(AppDbContext context, IRepositroy repositroy, IConfiguration configuration)
        {
            _context = context;
            _repository = repositroy;
            _connectionString = configuration.GetConnectionString("Menlyn_Mews"); // Access connection string from app settings

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

        // New endpoint for backing up the database
        [HttpPost]
        [Route("BackupDatabase")]
        public IActionResult BackupDatabase()
        {
            // Automatically generate a backup file name based on the current timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupFileName = $"Menlyn_Mews_Backup_{timestamp}.bak";
            string backupFilePath = Path.Combine("C:\\Backup", backupFileName); // Set your desired backup path

            string query = $"BACKUP DATABASE [Menlyn_Mews] TO DISK = '{backupFilePath}' WITH NOFORMAT, NOINIT, NAME = 'Menlyn_Mews Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return Ok($"Backup successful! File: {backupFileName}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Backup failed: {ex.Message}");
            }
        }

        // New endpoint for restoring the database
        [HttpPost]
        [Route("RestoreDatabase")]
        public IActionResult RestoreDatabase()
        {
            try
            {
                // Automatically get the most recent backup file
                var backupDirectory = new DirectoryInfo("C:\\Backup");
                var latestBackupFile = backupDirectory.GetFiles("Menlyn_Mews_Backup_*.bak")
                                        .OrderByDescending(f => f.LastWriteTime)
                                        .FirstOrDefault();

                if (latestBackupFile == null)
                {
                    return BadRequest("No backup file found.");
                }

                string backupFilePath = latestBackupFile.FullName;

                string restoreQuery = $@"
        USE master;
        ALTER DATABASE Menlyn_Mews SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        RESTORE DATABASE Menlyn_Mews FROM DISK = '{backupFilePath}' WITH REPLACE;
        ALTER DATABASE Menlyn_Mews SET MULTI_USER;";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(restoreQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                return Ok($"Database restored successfully from {latestBackupFile.Name}!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Restore failed: {ex.Message}");
            }
        }


        public class BackupViewModel
        {
            public string BackupFileName { get; set; }
        }


        private bool ComplaintExists(int id)
        {
            return (_context.Complaints?.Any(e => e.ComplaintId == id)).GetValueOrDefault();
        }
    }
}
