using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Inventory;
using Menlyn_Mews_API.Models.Domain.Emails;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Write_OffController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        private readonly IGeneralEmailService _generalEmailService;
        public Write_OffController(AppDbContext context, IRepositroy repositroy, IGeneralEmailService generalEmailService)
        {
            _context = context;
            _repository = repositroy;
            _generalEmailService = generalEmailService;
        }

        [HttpGet]
        [Route("GetWriteOffs")]
        public async Task<ActionResult> GetWrite_Offs()
        {
            try
            {
                var results = await _repository.GetWrite_OffsAsync();
                dynamic writeoffs = results.Select(wo => new
                {
                    wo.WriteOffId,
                    wo.Write_Off_Description,
                    wo.Quantity_Of_Items_Written_Off,
                    Inventory = wo.Room_Inventory.Inventory.Inventory_Name,
                    Inspection_Date = wo.Inspection_Item.Inspection_Date,
                    Employee = wo.Employee.Employee_Name + " " + wo.Employee.Employee_Surname,
                    Client_Name = wo.RoomBooking?.Clients?.Client_Name + " " + wo.RoomBooking?.Clients?.Client_Surname,
                    Room_Booked = wo.RoomBooking?.Rooms?.Room_Number
                });

                return Ok(writeoffs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetWriteOffById/{writeOffId}")]
        public async Task<ActionResult> GetWrite_Off(int writeOffId)
        {
            try
            {
                var wo = await _repository.GetWrite_OffByIdAsync(writeOffId);
                if (wo == null) return NotFound("Write Off Does Not Exist");

                dynamic writeoffs = new
                {
                    wo.WriteOffId,
                    wo.Write_Off_Description,
                    wo.Quantity_Of_Items_Written_Off,
                    wo.RoomId,
                    wo.InspectionItemId,   
                    wo.InventoryId,
                    wo.EmployeeId,
                    wo.RoomBooking.ClientId,
                    wo.RoomBookingId
                };

                return Ok(writeoffs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateWriteOff/{writeOffId}")]
        public async Task<ActionResult<WriteOffViewModel>> PutWrite_Off(int writeOffId, WriteOffViewModel wvm)
        {
            try
            {
                var writeoff = await _repository.GetWrite_OffByIdAsync(writeOffId);
                if (writeoff == null) return NotFound("Write-Off Does Not Exist");
                
                writeoff.Write_Off_Description = wvm.Write_Off_Description;
                writeoff.RoomId = wvm.RoomId;   
                writeoff.InspectionItemId = wvm.InspectionItemId;   
                writeoff.InventoryId = wvm.InventoryId;
                writeoff.EmployeeId = wvm.EmployeeId;
                //writeoff.ClientId = wvm.ClientId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(writeoff);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddWriteOff")]
        public async Task<IActionResult> PostWrite_Off(WriteOffViewModel wvm)
        {
            var writeoff = new Write_Off
            {
                Write_Off_Description = wvm.Write_Off_Description,
                RoomId = wvm.RoomId,
                InspectionItemId = wvm.InspectionItemId,
                InventoryId = wvm.InventoryId,
                EmployeeId = wvm.EmployeeId,
                Quantity_Of_Items_Written_Off = wvm.Quantity_Of_Items_Written_Off,
                RoomBookingId = wvm.RoomBookingId,
            };

            try
            {
                var test = await _repository.GetInventoryByIdAsync(writeoff.InventoryId);
                test.Quantity_Available -= writeoff.Quantity_Of_Items_Written_Off;
                _repository.Add(writeoff);

                var updateStatus = await _repository.GetInspectionItemsByIdAsync(writeoff.InspectionItemId);
                updateStatus.Inspection_Status = "Complete";

                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(writeoff);

        }

        [HttpPost]
        [Route("SendWriteOffInvoice/{writeOffId}")]
        public async Task<IActionResult> SendWriteOffEmail(int writeOffId)
        {
        var wo = await _repository.GetWrite_OffByIdAsync(writeOffId);
        if (wo == null) return NotFound("Write-Off Does Not Exist");

        try
        {
            var mailrequest = new Mailrequest
            {
                ToEmail = "ammaruh786@gmail.com",
                Subject = "Invoice for Damaged Items During Your Stay at Menlyn Mews",
                Body = GenerateEmailBody(wo)
            };

            await _generalEmailService.SendEmailAsync(mailrequest);
            return Ok();
        }
        catch (Exception ex)
        {
            throw;
        }
        }

        private string GenerateEmailBody(Write_Off wo)
        {
            string body = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        color: #333;
                        background-color: blue;
                    }}
                    .header {{
                        text-align: center;
                        margin-bottom: 20px;
                    }}
                    .content {{
                        margin: 0 15%;
                    }}
                    .footer {{
                        text-align: center;
                        margin-top: 20px;
                        color: #888;
                    }}
                </style>
            </head>
            <body>
                <div class='header'>
                    <img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT89VWgW6STd_zdvJN2_Syu9YPCdnbGaPfKkw&s' alt='Menlyn Mews' width='150'>
                    <h2>Invoice for Damaged Items</h2>
                </div>
                <div class='content'>
                    <p>Dear ,</p>
                    <p>We hope you enjoyed your stay at Menlyn Mews. Unfortunately, it has come to our attention that the following items were damaged during your stay:</p>
                    <p><strong>Description:</strong> {wo.Write_Off_Description}</p>
                    <p><strong>Quantity:</strong> {wo.Quantity_Of_Items_Written_Off}</p>
                    <p>We kindly request that you cover the cost to replace these items. Please find the invoice attached.</p>
                    <p>If you have any questions or need further assistance, feel free to contact us.</p>
                    <p>Thank you for your understanding and cooperation.</p>
                    <p>Best regards,</p>
                    <p>The Menlyn Mews Team</p>
                </div>
                <div class='footer'>
                    <p>Menlyn Mews | 226 Frikkie De Beer St, Menlyn, Pretoria, 0063 | +27 64 504 7520</p>
                </div>
            </body>
            </html>";

            return body;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWrite_Off(int id)
        {
            if (_context.Write_Offs == null)
            {
                return NotFound();
            }
            var write_Off = await _context.Write_Offs.FindAsync(id);
            if (write_Off == null)
            {
                return NotFound();
            }

            _context.Write_Offs.Remove(write_Off);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Write_OffExists(int id)
        {
            return (_context.Write_Offs?.Any(e => e.WriteOffId == id)).GetValueOrDefault();
        }
    }
}
