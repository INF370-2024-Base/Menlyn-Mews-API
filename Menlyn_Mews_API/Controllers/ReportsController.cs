using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _productRepositroy;

        public ReportsController(AppDbContext context, IRepositroy productRepositroy)
        {
            _context = context;
            _productRepositroy = productRepositroy;
        }

        [HttpGet]
        [Route("ProductsReport")]
        public async Task<ActionResult<dynamic>> ProductsReport()
        {
            try
            {
                List<dynamic> productsreport = new List<dynamic>();

                var results = await _productRepositroy.GetProductsAsync();

                dynamic productCategory = results
                                        .GroupBy(p => p.ProductCategory.Product_Category_Name)
                                        .Select(c => new
                                        {
                                            Key = c.Key,
                                            ProductCount = c.Count()
                                        });

                dynamic productList = results
                                    .GroupBy(p => new { CategoryName = p.ProductCategory.Product_Category_Name, ProductName = p.Product_Name })
                                    .Select(p => new
                                    {
                                        p.Key.CategoryName,
                                        p.Key.ProductName,
                                    });

                productsreport.Add(productCategory);
                productsreport.Add(productList);

                return productsreport;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }           
        }
    }
}
