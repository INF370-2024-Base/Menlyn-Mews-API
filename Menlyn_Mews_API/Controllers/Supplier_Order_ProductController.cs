using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Supplier;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Supplier_Order_ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Supplier_Order_ProductController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetOrderProducts")]
        public async Task<ActionResult> GetSupplier_Order_Products()
        {
            try
            {
                var results = await _repository.GetSupplierOrderProductAsync();

                dynamic orderProducts = results.Select(op => new
                {
                    op.OrderId,
                    op.ProductId,
                    op.Quantity,
                    op.Order.Order_Description,
                    op.Product.Product_Name,
                    //op.Product.Price.Product_Price,
                    op.Receive_Order?.Received_By,
                    op.Receive_Order?.Received_Order_Date
                });

                return Ok(orderProducts);   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("FilterProducts/{orderId}")]
        public async Task<IActionResult> FilterProducts(int orderId)
        {
            try
            {
                var orderProducts = await _repository.FilterProductsByOrderIdAsync(orderId);

                if (orderProducts == null)
                    return NotFound("Order Does Not Exist");

                var allProducts = await _repository.GetProductsAsync();

                var orderProductIds = orderProducts.Select(p => p.ProductId).ToHashSet();

                var filteredProducts = allProducts.Where(p => !orderProductIds.Contains(p.ProductId));

                dynamic results = filteredProducts.Select(p => new
                {
                    p.ProductId,
                    p.Product_Name
                });

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetOrderWithProducts/{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                var results = await _repository.FilterProductsByOrderIdAsync(orderId);

                if (results == null || !results.Any())
                {
                    return NotFound("Order Does Not Exist");
                }

                var order = results
                    .Where(o => o.OrderId == orderId)
                    .GroupBy(o => o.OrderId)
                    .Select(g => new
                    {
                        OrderId = g.Key,
                        Products = string.Join(", ", g.Select(p => $"{p.Product.Product_Name} x{p.Quantity}"))
                    })
                    .FirstOrDefault();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpGet]
        [Route("GetOrderProductById/{orderId}/{productId}")]
        public async Task<ActionResult> GetSupplier_Order_Product(int orderId, int productId)
        {
            try
            {
                var op = await _repository.GetSupplierOrderProductByIdAsync(orderId, productId);
                if (op == null) return NotFound("Order Does Not Exist");

                dynamic orderProduct = new
                {
                    op.OrderId,
                    op.ProductId,
                    op.Quantity,
                    op.Order.Order_Description,
                    op.Product.Product_Name,
                   // op.Product.Price.Product_Price,
                    op.Receive_Order?.Received_By,
                    op.Receive_Order?.Received_Order_Date
                };

                return Ok(orderProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateOrderProduct/{orderId}/{productId}")]
        public async Task<ActionResult<OrderProductViewModel>> PutSupplier_Order_Product(int orderId, int productId, OrderProductViewModel opvm)
        {
            try
            {
                var orderProduct = await _repository.GetSupplierOrderProductByIdAsync(orderId, productId);
                if (orderProduct == null) return NotFound("Order Does Not Exist");

                orderProduct.OrderId = orderId; 
                orderProduct.ProductId = productId; 
                orderProduct.Quantity = opvm.Quantity;
                orderProduct.ReceiveOrderId = opvm.ReceiveOrderId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(orderProduct);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("CreateOrderProduct")]
        public async Task<IActionResult> PostSupplier_Order_Product(OrderProductViewModel opvm)
        {
            var orderProduct = new Supplier_Order_Product
            {
                OrderId = opvm.OrderId, 
                ProductId = opvm.ProductId,
                Quantity = opvm.Quantity,
                ReceiveOrderId = opvm.ReceiveOrderId,   
            };

            try
            {
                _repository.Add(orderProduct);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(orderProduct);
        }

        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> DeleteSupplier_Order_Product(int orderId, int productId)
        {
            if (_context.Supplier_Order_Products == null)
            {
                return NotFound();
            }
            var supplier_Order_Product = await _context.Supplier_Order_Products.FindAsync(orderId, productId);
            if (supplier_Order_Product == null)
            {
                return NotFound();
            }

            _context.Supplier_Order_Products.Remove(supplier_Order_Product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Supplier_Order_ProductExists(int id)
        {
            return (_context.Supplier_Order_Products?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
