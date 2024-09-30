using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Supplier;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Receive_OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Receive_OrderController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetReceivedOrders")]
        public async Task<ActionResult> GetReceive_Orders()
        {
            try
            {
                var receievedOrders = await _repository.GetReceivedOrdersAsync();
                return Ok(receievedOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetReceivedOrderById/{receiveOrderId}")]
        public async Task<ActionResult> GetReceive_Order(int receiveOrderId)
        {
            try
            {
                var receievedOrders = await _repository.GetReceivedOrdersByIdAsync(receiveOrderId);
                if (receievedOrders == null) return NotFound("Receivd Order Does Not Exist");
                return Ok(receievedOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateReceivedOrder/{receiveOrderId}")]
        public async Task<ActionResult<ReceiveOrderViewModel>> PutReceive_Order(int receiveOrderId, ReceiveOrderViewModel rvm)
        {
            try
            {
                var receievedOrders = await _repository.GetReceivedOrdersByIdAsync(receiveOrderId);
                if (receievedOrders == null) return NotFound("Receivd Order Does Not Exist");

                receievedOrders.Received_By = rvm.Received_By;
                receievedOrders.Received_Status = rvm.Received_Status;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(receievedOrders);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("AddReceievedOrder/{orderId}")]
        public async Task<IActionResult> PostReceive_Order(ReceiveOrderViewModel rvm, int orderId)
        {
            var receievedOrder = new Receive_Order
            {
                Received_Order_Date = DateTime.Now,
                Received_By = rvm.Received_By,
                Received_Status = rvm.Received_Status
            };

            try
            {
                _repository.Add(receievedOrder);
                await _repository.SaveChangesAsync();   


                foreach (var productReceived in rvm.ProductsReceived)
                {
                    var product = await _repository.GetProductAsync(productReceived.ProductId);
                    if (product != null)
                    {
                        var inventory = await _repository.GetInventoryByProductNameAsync(product.Product_Name);
                        if (inventory != null) 
                        {
                            inventory.Quantity_Available += productReceived.QuantityReceived;
                        }
                    }
                    await _repository.SaveChangesAsync();
                }

                var orderStatus = await _repository.GetOrderByIdAsync(orderId);
                orderStatus.Order_Status = rvm.Received_Status;

                await _repository.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(receievedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceive_Order(int id)
        {
            if (_context.Receive_Orders == null)
            {
                return NotFound();
            }
            var receive_Order = await _context.Receive_Orders.FindAsync(id);
            if (receive_Order == null)
            {
                return NotFound();
            }

            _context.Receive_Orders.Remove(receive_Order);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
