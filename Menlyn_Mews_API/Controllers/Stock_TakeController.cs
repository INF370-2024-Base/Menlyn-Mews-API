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
    public class Stock_TakeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Stock_TakeController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetStockTakes")]
        public async Task<ActionResult> GetStock_Takes()
        {
            try
            {
                var results = await _repository.GetStockTakesAsync();

                dynamic stocktakes = results.Select(st => new
                {
                    st.StockTakeId,
                    st.Stock_Take_Date,
                    st.Total_Items,
                    st.Total_Value,
                    Employee = st.Employee_Shift.Employee.Employee_Name + " " + st.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date = st.Employee_Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (st.Employee_Shift.Shift.Start_TIme.HasValue && st.Employee_Shift.Shift.End_TIme.HasValue)
                          ? st.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + st.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,
                    Inventory = st.Inventory.Inventory_Name,
                });

                return Ok(stocktakes);
            }
            catch (Exception ex)
            {   
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStockTakeById/{stockTakeId}")]
        public async Task<ActionResult> GetStock_Take(int stockTakeId)
        {
            try
            {
                var st = await _repository.GetStockTakeByIdAsync(stockTakeId);
                if (st == null) return NotFound("Stock Take Does Not Exist");

                dynamic stocktakes = new
                {
                    st.StockTakeId,
                    st.Stock_Take_Date,
                    st.Total_Items,
                    st.Total_Value,
                    Employee = st.Employee_Shift.Employee.Employee_Name + " " + st.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date = st.Employee_Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (st.Employee_Shift.Shift.Start_TIme.HasValue && st.Employee_Shift.Shift.End_TIme.HasValue)
                          ? st.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + st.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,
                    Inventory = st.Inventory.Inventory_Name,
                };

                return Ok(stocktakes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateStockTake/{stockTakeId}")]
        public async Task<ActionResult<StockTakeViewModel>> PutStock_Take(int stockTakeId, StockTakeViewModel svm)
        {
            try
            {
                var stocktake = await _repository.GetStockTakeByIdAsync(stockTakeId);
                if (stocktake == null) return NotFound("Stock Take Does Not Exist");

                stocktake.Stock_Take_Date = svm.Stock_Take_Date;    
                stocktake.Total_Items = svm.Total_Items;    
                stocktake.Total_Value = svm.Total_Value;
                stocktake.EmployeeId = svm.EmployeeId;
                stocktake.ShiftId = svm.ShiftId;
                stocktake.InventoryId = svm.InventoryId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(stocktake);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddStockTake")]
        public async Task<IActionResult> PostStock_Take(StockTakeViewModel svm)
        {
            var stocktake = new Stock_Take
            {
                Stock_Take_Date = svm.Stock_Take_Date,
                Total_Items = svm.Total_Items,
                Total_Value = svm.Total_Value,  
                EmployeeId = svm.EmployeeId,
                ShiftId = svm.ShiftId,
                InventoryId = svm.InventoryId,  
            };
            if (await _context.Employee_Shifts.AnyAsync(es => es.EmployeeId == stocktake.EmployeeId && es.ShiftId == stocktake.ShiftId))
            {
                try
                {
                    _repository.Add(stocktake);
                    await _repository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok(stocktake);
            }
            else
            {
                return NotFound("The Employee Shift Does Not Exist");
            }

        }

        // DELETE: api/Stock_Take/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock_Take(int id)
        {
            if (_context.Stock_Takes == null)
            {
                return NotFound();
            }
            var stock_Take = await _context.Stock_Takes.FindAsync(id);
            if (stock_Take == null)
            {
                return NotFound();
            }

            _context.Stock_Takes.Remove(stock_Take);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Stock_TakeExists(int id)
        {
            return (_context.Stock_Takes?.Any(e => e.StockTakeId == id)).GetValueOrDefault();
        }
    }
}
