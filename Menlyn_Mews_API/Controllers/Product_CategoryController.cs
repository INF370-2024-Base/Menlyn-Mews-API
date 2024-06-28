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
using Menlyn_Mews_API.ViewModels.Products;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product_CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public Product_CategoryController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        // GET: api/Product_Category
        [HttpGet]
        [Route("GetProductCategories")]
        public async Task<ActionResult> GetProduct_Categories()
        {
            try
            {
                var productCategories = await _repository.GetProductCategoriesAsync();
                return Ok(productCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product_Category>> GetProduct_Category(int id)
        {
            if (_context.Product_Categories == null)
            {
                return NotFound();
            }
            var product_Category = await _context.Product_Categories.FindAsync(id);

            if (product_Category == null)
            {
                return NotFound();
            }

            return product_Category;
        }

        [HttpPut]
        [Route("UpdateProductCategory/{productCategoryId}")]
        public async Task<ActionResult<ProductCategoryViewModel>> PutProduct_Category(int productCategoryId, ProductCategoryViewModel pcvm)
        {
            try
            {
                var productCategory = await _repository.GetProductCategoryByIdAsync(productCategoryId);
                if (productCategory == null) return NotFound("Product Category Does Not Exist");

                productCategory.Product_Category_Name = pcvm.Product_Category_Name;
                productCategory.Product_Category_Description = pcvm.Product_Category_Description;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(productCategory);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest("Invalid Request");
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct_Category(ProductCategoryViewModel pcvm)
        {
            var productCategory = new Product_Category
            {
                Product_Category_Name = pcvm.Product_Category_Name,
                Product_Category_Description = pcvm.Product_Category_Description,
            };

            try
            {
                _repository.Add(productCategory);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(productCategory);
        }

        // DELETE: api/Product_Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct_Category(int id)
        {
            if (_context.Product_Categories == null)
            {
                return NotFound();
            }
            var product_Category = await _context.Product_Categories.FindAsync(id);
            if (product_Category == null)
            {
                return NotFound();
            }

            _context.Product_Categories.Remove(product_Category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Product_CategoryExists(int id)
        {
            return (_context.Product_Categories?.Any(e => e.ProductCategoryId == id)).GetValueOrDefault();
        }
    }
}
