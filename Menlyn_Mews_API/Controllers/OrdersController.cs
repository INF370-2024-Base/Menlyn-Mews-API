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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public OrdersController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetOrders")]
        public async Task<ActionResult> GetOrders()
        {
            try
            {
                var results = await _repository.GetOrdersAsync();

                dynamic orders = results.Select(o => new
                {
                    o.OrderId,
                    o.Order_Description,
                    o.Order_Date,
                    o.Order_Status,
                    Supplier = o.Suppliers.Supplier_Name,
                    Employee = o.Employee_Shift.Employee.Employee_Name + " " + o.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date = o.Employee_Shift.Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (o.Employee_Shift.Shift.Start_TIme.HasValue && o.Employee_Shift.Shift.End_TIme.HasValue)
                          ? o.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + o.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,

                });

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetOrderById/{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(int orderId)
        {
            try
            {
                var o = await _repository.GetOrderByIdAsync(orderId);
                if (o == null) return NotFound("Order Does Not Exist");

                dynamic orders = new
                {
                    o.OrderId,
                    o.Order_Description,
                    o.Order_Date,
                    o.Order_Status,
                    Supplier = o.Suppliers.Supplier_Name,
                    Employee = o.Employee_Shift.Employee.Employee_Name + " " + o.Employee_Shift.Employee.Employee_Surname,
                    Shift_Date = o.Employee_Shift.Shift.Shift_Date.Date.ToString("yyyy-MM-dd"),
                    Shift_Time = (o.Employee_Shift.Shift.Start_TIme.HasValue && o.Employee_Shift.Shift.End_TIme.HasValue)
                          ? o.Employee_Shift.Shift.Start_TIme.Value.ToString("hh:mm tt") + " - " + o.Employee_Shift.Shift.End_TIme.Value.ToString("hh:mm tt")
                          : string.Empty,

                };

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateOrder/{orderId}")]
        public async Task<ActionResult<OrderViewModel>> PutOrder(int orderId, OrderViewModel ovm)
        {
            try
            {
                var o = await _repository.GetOrderByIdAsync(orderId);
                if (o == null) return NotFound("Order Does Not Exist");

                o.Order_Description = ovm.Order_Description;
                o.Order_Date = ovm.Order_Date;
                o.Order_Status = ovm.Order_Status;
                o.SupplierId = ovm.SupplierId;
                o.EmployeeId = ovm.EmployeeId;
                o.ShiftId = ovm.ShiftId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(o);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<IActionResult> PostOrder(OrderViewModel ovm)
        {
            var order = new Order
            {
                Order_Description = ovm.Order_Description,
                Order_Date = ovm.Order_Date,
                Order_Status = ovm.Order_Status,
                SupplierId = ovm.SupplierId,
                EmployeeId = ovm.EmployeeId,
                ShiftId = ovm.ShiftId
            };

            try
            {
                _repository.Add(order);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
