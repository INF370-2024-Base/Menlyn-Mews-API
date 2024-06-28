using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Models.Repositories
{
    public class Repository : IRepositroy
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }


        ///////////////////////////////////////////////////////PRODUCT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////
        //Products
        public async Task<Product[]> GetProductsAsync()
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Inventory).Include(p => p.ProductType).Include(p => p.ProductCategory).Include(p => p.Price);

            return await query.ToArrayAsync();
        }

        public async Task<Product[]> GetProductsReportAsync()
        {
            IQueryable<Product> query = _context.Products.Include(p => p.ProductCategory);

            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            IQueryable<Product> query = _context.Products.Where(p => p.ProductId == productId).Include(p => p.ProductType).Include(p => p.ProductCategory).Include(p => p.Price);
            return await query.FirstOrDefaultAsync();
        }

        //Product Type
        public async Task<Product_Type[]> GetProductTypesAsync()
        {
            IQueryable<Product_Type> query = _context.Product_Types;
            return await query.ToArrayAsync();  
        }

        public async Task<Product_Type> GetProductTypeByIdAsync(int productTypeId)
        {
            IQueryable<Product_Type> query = _context.Product_Types.Where(pt => pt.ProductTypeId == productTypeId);
            return await query.FirstOrDefaultAsync();
        }

        //Product Category
        public async Task<Product_Category[]> GetProductCategoriesAsync()
        {
            IQueryable<Product_Category> query =  _context.Product_Categories;
            return await query.ToArrayAsync();
        }

        public async Task<Product_Category> GetProductCategoryByIdAsync(int categoryId)
        {
            IQueryable<Product_Category> query = _context.Product_Categories.Where(pc => pc.ProductCategoryId == categoryId);
            return await query.FirstOrDefaultAsync();
        }


        ///////////////////////////////////////////////////////PRODUCT REPOSITORY END/////////////////////////////////////////////////////////////////////////////////////
    }

}
