using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Models.Repositories
{
    public interface IRepositroy
    {
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        ///////////////////////////////////////////////////////PRODUCT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////
        //Product
        Task<Product[]> GetProductsReportAsync();

        Task<Product[]> GetProductsAsync();
        Task<Product> GetProductAsync(int productId);

        //Product Type
        Task<Product_Type[]> GetProductTypesAsync();
        Task<Product_Type> GetProductTypeByIdAsync(int productTypeId);

        //Product Category
        Task<Product_Category[]> GetProductCategoriesAsync();
        Task<Product_Category> GetProductCategoryByIdAsync(int categoryId);

        ///////////////////////////////////////////////////////PRODUCT REPOSITORY END////////////////////////////////////////////////////////////////////////////////////

    }
}
