using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Products;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product_TypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Product_TypeController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]
        [Route("GetProductTypes")]
        public async Task<ActionResult> GetProduct_Types()
        {
            try
            {
                var results = await _repository.GetProductTypesAsync();

                dynamic products = results.Select(pt => new
                {
                    pt.ProductTypeId,
                    pt.Product_Type_Description,
                    pt.Product_Type_Name,
                    pt.ProductCategory.Product_Category_Name
                });

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetProductTypeById/{productTypeId}")]
        public async Task<ActionResult<Product_Type>> GetProduct_Type(int productTypeId)
        {
            try
            {
                var results = await _repository.GetProductTypeByIdAsync(productTypeId);
                if (results == null) return NotFound("Product Type Does Not Exist");

                dynamic products =  new
                {
                    results.ProductTypeId,
                    results.Product_Type_Description,
                    results.Product_Type_Name,  
                    results.ProductCategoryId,
                };

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetProductTypeByCategoryId/{productCategoryId}")]
        public async Task<ActionResult<Product_Type>> GetProductTypeByCategory(int productCategoryId)
        {
            try
            {
                var results = await _repository.GetProductTypesByCategoryAsync(productCategoryId);

                dynamic products = results.Select(pt => new
                {
                    pt.ProductTypeId,
                    pt.Product_Type_Description,
                    pt.Product_Type_Name,
                    pt.ProductCategory.Product_Category_Name
                });

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("EditProductType/{productTypeId}")]
        public async Task<ActionResult<ProductTypeViewModel>> PutProduct_Type(int productTypeId, ProductTypeViewModel ptvm)
        {
            try
            {
                var productType = await _repository.GetProductTypeByIdAsync(productTypeId);
                if (productType == null) return NotFound("Product Type Does Not Exist");

                productType.Product_Type_Name = ptvm.Product_Type_Name;
                productType.Product_Type_Description = ptvm.Product_Type_Description;
                productType.ProductCategoryId = ptvm.ProductCategoryId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(productType);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest("Invalid Request");
        }

        [HttpPost]
        [Route("AddProductType")]
        public async Task<IActionResult> PostProduct_Type(ProductTypeViewModel ptvm)
        {
            var productType = new Product_Type
            {
                Product_Type_Name = ptvm.Product_Type_Name,
                Product_Type_Description = ptvm.Product_Type_Description,
                ProductCategoryId = ptvm.ProductCategoryId,
            };

            try
            {
                _repository.Add(productType);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(productType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct_Type(int id)
        {
            if (_context.Product_Types == null)
            {
                return NotFound();
            }
            var product_Type = await _context.Product_Types.FindAsync(id);
            if (product_Type == null)
            {
                return NotFound();
            }

            _context.Product_Types.Remove(product_Type);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Product_TypeExists(int id)
        {
            return (_context.Product_Types?.Any(e => e.ProductTypeId == id)).GetValueOrDefault();
        }
    }
}
