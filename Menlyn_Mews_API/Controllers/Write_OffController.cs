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
using Menlyn_Mews_API.ViewModels.Inventory;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Write_OffController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Write_OffController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
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
                    wo.Write_Off_Date,
                    wo.Write_Off_Stock_Type_Name,
                    wo.Write_Off_Stock_Type_Description,
                    Inventory = wo.Inventory.Inventory_Name,
                    Employee = wo.Employee_Shift.Employee.Employee_Name + " " + wo.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date =  wo.Employee_Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (wo.Employee_Shift.Shift.Start_TIme.HasValue && wo.Employee_Shift.Shift.End_TIme.HasValue)
                          ? wo.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + wo.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,
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
                    wo.Write_Off_Date,
                    wo.Write_Off_Stock_Type_Name,
                    wo.Write_Off_Stock_Type_Description,
                    Inventory = wo.Inventory.Inventory_Name,
                    Employee = wo.Employee_Shift.Employee.Employee_Name + " " + wo.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date = wo.Employee_Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (wo.Employee_Shift.Shift.Start_TIme.HasValue && wo.Employee_Shift.Shift.End_TIme.HasValue)
                          ? wo.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + wo.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,
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
                writeoff.Write_Off_Date = wvm.Write_Off_Date;
                writeoff.Write_Off_Stock_Type_Name = wvm.Write_Off_Stock_Type_Name;
                writeoff.Write_Off_Stock_Type_Description = wvm.Write_Off_Stock_Type_Description;
                writeoff.InventoryId = wvm.InventoryId;
                writeoff.EmployeeId = wvm.EmployeeId;
                writeoff.ShiftId = wvm.ShiftId;

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
                Write_Off_Date = wvm.Write_Off_Date,
                Write_Off_Stock_Type_Name = wvm.Write_Off_Stock_Type_Name,
                Write_Off_Stock_Type_Description = wvm.Write_Off_Stock_Type_Description,
                InventoryId = wvm.InventoryId,  
                EmployeeId = wvm.EmployeeId,    
                ShiftId = wvm.ShiftId,  
            };

            if (await _context.Employee_Shifts.AnyAsync(es => es.EmployeeId == writeoff.EmployeeId && es.ShiftId == writeoff.ShiftId))
            {
                try
                {
                    _repository.Add(writeoff);
                    await _repository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok(writeoff);
            }
            else
            {
                return NotFound("The Employee Shift Does Not Exist");
            }


        }

        // DELETE: api/Write_Off/5
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
