using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Models.Repositories
{
    public interface IRepositroy
    {
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<VAT[]> GetVATAsync();
        Task<VAT> GetVATByIdAsync(int vatId);

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

        //Price
        Task<Price[]> GetPricesAsync();
        Task<Price> GetPriceByIdAsync(int priceId);

        ///////////////////////////////////////////////////////PRODUCT REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////INVENTORY REPOSITORY////////////////////////////////////////////////////////////////////////////////////////
        //Inventory
        Task<Inventory[]> GetInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int inventoryId);

        //Inventory Type
        Task<Inventory_Type[]> GetInventoryTypesAsync();
        Task<Inventory_Type> GetInventoryTypesByIdAsync(int inventoryTypeId);

        //Inventory Category
        Task<Inventory_Category[]> GetInventoryCategoriesAsync();
        Task<Inventory_Category> GetInventoryCategoriesByIdAsync(int inventoryCategoryId);

        //Inspection Item
        Task<Inspection_Item[]> GetInspectionItemsAsync();
        Task<Inspection_Item> GetInspectionItemsByIdAsync(int inspectionItemId);

        //Write-Offs
        Task<Write_Off[]> GetWrite_OffsAsync(); 
        Task<Write_Off> GetWrite_OffByIdAsync(int writeOffId);

        //Stock Take
        Task<Stock_Take[]> GetStockTakesAsync();
        Task<Stock_Take> GetStockTakeByIdAsync(int stockTakeId);

        ///////////////////////////////////////////////////////INVENTORY REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////EMPLOYEE REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

        //Position
        public Task<Position[]> GetPositionsAsync();
        public Task<Position> GetPositionByIdAsync(int positionId);

        //Employee Type
        public Task<Employee_Type[]> GetEmployeeTypesAsync();   
        public Task<Employee_Type> GetEmployeeTypeByIdAsync(int employeeTypeId);

        ///////////////////////////////////////////////////////EMPLOYEE REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////BOOKING REPOSITORY////////////////////////////////////////////////////////////////////////////////////////
        
        //Booking Package
        public Task<Booking_Package[]> GetBookingPackagesAsync();
        public Task<Booking_Package> GetBookingPackageByIdAsync(int bookingPackageId);

        //Discount
        public Task<Discount[]> GetDiscountsAsync();
        public Task<Discount> GetDiscountByIdAsync(int discountId);

        //Room Type
        public Task<Room_Type[]> GetRoomTypesAsync();
        public Task<Room_Type> GetRoomTypeByIdAsync(int roomTypeId);

        ///////////////////////////////////////////////////////BOOKING REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////CLIENT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

        //Event Review
        public Task<Event_Review[]> GetEventReviewsAsync();
        public Task<Event_Review> GetEventReviewByIdAsync(int eventReviewId);

        //Booking Review
        public Task<Booking_Review[]> GetBookingReviewsAsync();
        public Task<Booking_Review> GetBookingReviewByIdAsync(int bookingReviewId);

        ///////////////////////////////////////////////////////CLIENT REPOSITORY END////////////////////////////////////////////////////////////////////////////////////
    }
}
