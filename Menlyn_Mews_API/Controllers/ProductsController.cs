using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Products;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public ProductsController(AppDbContext context, IRepositroy productRepositroy)
        {
            _context = context;
            _repository = productRepositroy;
        }

        // GET: api/Products
        [HttpGet]
        [Route("GetProducts")]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                var results = await _repository.GetProductsAsync();

                dynamic products = results.Select(p => new
                {
                    p.ProductId,
                    p.Product_Name,
                    p.Quantity_On_Hand,
                    Type_Name = p.ProductType.Product_Type_Name,
                    Category_Name = p.ProductType.ProductCategory.Product_Category_Name,
                    Inventory = p.Inventory.Inventory_Name,
                    Price  = p.Price.Product_Price
                });

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetProductById/{productId}")]
        public async Task<ActionResult> GetProduct(int productId)
        {
            try
            {
                var p = await _repository.GetProductAsync(productId);
                if (p == null) return NotFound("Product Does Not Exist");
                
                dynamic products = new
                {
                    p.ProductId,
                    p.Product_Name,
                    p.Quantity_On_Hand,
                    Inventory_Id = p.InventoryId,
                    Product_Type_Id = p.ProductTypeId,
                    Price_Id = p.PriceId,
                    p.ProductType.ProductCategory.ProductCategoryId,
                    p.Price.Product_Price
                };

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("EditProduct/{productId}")]
        public async Task<ActionResult<ProductViewModel>> PutProduct(int productId, ProductViewModel pvm)
        {
            try
            {
                var existingProduct = await _repository.GetProductAsync(productId);
                if (existingProduct == null) return NotFound("The Product Does Not Exist");

                existingProduct.Product_Name = pvm.Product_Name;
                existingProduct.Quantity_On_Hand = pvm.Quantity_On_Hand;
                existingProduct.PriceId = pvm.Price_Id;
                existingProduct.InventoryId = pvm.Inventory_Id;
                existingProduct.ProductTypeId = pvm.Product_Type_Id;
                existingProduct.PriceId = pvm.Price_Id;
                existingProduct.Price.Product_Price = pvm.Product_Price;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingProduct);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");
        }

        // POST: api/Products
        [HttpPost]
        [Route("PostProduct")]
        public async Task<IActionResult> PostProduct(AddProductViewModel pvm)
        {
            var price = new Price
            {
                Product_Price = pvm.Product_Price,
                Price_Date = DateTime.Now,
            };

            try
            {
                _repository.Add(price);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid Price Transaction");
            }

            var product = new Product
            {
                Product_Name = pvm.Product_Name,
                Quantity_On_Hand = pvm.Quantity_On_Hand,
                ProductTypeId = pvm.Product_Type_Id,
                InventoryId = pvm.Inventory_Id,
                PriceId = price.PriceId,
            };

            try
            {
                _repository.Add(product);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
