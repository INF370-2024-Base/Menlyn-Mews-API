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

        ///////////////////////////////////////////////////////REPORT REPOSITORY/////////////////////////////////////////////////////////////////////////////////////////
        Task<Product[]> GetProductsReportAsync();

        ///////////////////////////////////////////////////////REPORT REPOSITORY END/////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////PRODUCT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////
        
        //Product
        Task<Product[]> GetProductsAsync();
        Task<Product> GetProductAsync(int productId);

        //Product Type
        Task<Product_Type[]> GetProductTypesAsync();
        Task<Product_Type> GetProductTypeByIdAsync(int productTypeId);
        Task<Product_Type[]> GetProductTypesByCategoryAsync(int productCategoryId);

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

        //Room Inventory
        Task<Room_Inventory[]> GetRoomInventoriesAsync();
        Task<Room_Inventory> GetRoomInventoryByIdAsync(int roomId, int inventoryId);

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

        //Shift
        public Task<Shift[]> GetShiftsAsync();  
        public Task<Shift> GetShiftByIdAsync(int shiftId);

        //Employee
        public Task<Employee[]> GetEmployeesAsync();
        public Task<Employee> GetEmployeeByIdAsync(int employeeId);
        public Task<Employee> GetEmployeeByAppUserIdAsync(string appUserId);

        //Employee_Shift
        public Task<Employee_Shift[]> GetEmployeeShiftsAsync();
        public Task<Employee_Shift[]> GetEmployeeShiftWithRateAsync();
        public Task<Employee_Shift> GetEmployeeShiftByIdAsync(int employeeId, int shiftId);
        public Task<Employee_Shift> GetEmployeeShiftByIdEmployeeAsync(int employeeId);

        //Rate
        public Task<Rates[]> GetRatesAsync();
        public Task<Rates> GetRatesByIdAsync(int ratesId);

        ///////////////////////////////////////////////////////EMPLOYEE REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////BOOKING REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

        //Booking Package
        public Task<Booking_Package[]> GetBookingPackagesAsync();
        public Task<Booking_Package> GetBookingPackageByIdAsync(int bookingPackageId);

        //Discount
        public Task<Discount[]> GetDiscountsAsync();
        public Task<Discount> GetDiscountByIdAsync(int discountId);
        public Task<Discount> FindDiscountCodeAsync(string code);  

        //Room Type
        public Task<Room_Type[]> GetRoomTypesAsync();
        public Task<Room_Type> GetRoomTypeByIdAsync(int roomTypeId);

        //Room
        public Task<Room[]> GetRoomsAsync();
        public Task<Room> GetRoomByIdAsync(int roomId);
        public Task<Room_Inventory[]> FilterInventoriesByRoomIdAsync(int roomId);

        //Room Booking
        public Task<Room_Booking[]> GetRoomBookingsAsync();
        public Task<Room_Booking> GetRoomBookingByIdAsync(int bookingId);
        public Task<Room_Booking[]> GetRoomBookingByClientIdAsync(int clientId);
        public Task<Room_Booking[]> GetBookedRooms(int roomId);

        ///////////////////////////////////////////////////////BOOKING REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////CLIENT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

        //Get Client By AppUserId
        public Task<Client> GetClientByAppUserIdAsync(string appUserId);

        //Event Review
        public Task<Event_Review[]> GetEventReviewsAsync();
        public Task<Event_Review> GetEventReviewByIdAsync(int eventReviewId);

        //Booking Review
        public Task<Booking_Review[]> GetBookingReviewsAsync();
        public Task<Booking_Review> GetBookingReviewByIdAsync(int bookingReviewId);
        public Task<Booking_Review[]> GetReviewsByRoomIdAsync(int roomId);

        //Client
        public Task<Client[]> GetClientsAsync();
        public Task<Client> GetClientByIdAsync(int clientId);

        //Complaint Type
        public Task<Complaint_Type[]> GetComplaintTypesAsync();
        public Task<Complaint_Type> GetComplaintTypeByIdAsync(int complaintTypeId);

        //Complaint
        public Task<Complaint[]> GetComplaintsAsync();
        public Task<Complaint> GetComplaintByIdAsync(int complaintId);

        //Client Discount
        public Task<Client_Discount[]> GetClientDiscountsAsync();
        public Task<Client_Discount> GetClientDiscountByIdAsync(int discountId, int clientId);
        public Task<Client_Discount[]> GetUsedClientDiscountsAsync(int clientId);

        ///////////////////////////////////////////////////////CLIENT REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////SUPPLIER REPOSITORY///////////////////////////////////////////////////////////////////////////////////////

        //Supplier Type
        public Task<Supplier_Type[]> GetSupplierTypesAsync();
        public Task<Supplier_Type> GetSupplierTypeByIdAsync(int supplierTypeId);

        //Supplier
        public Task<Supplier[]> GetSuppliersAsync();    
        public Task<Supplier> GetSupplierByIdAsync(int supplierId);

        //Order
        public Task<Order[]> GetOrdersAsync();
        public Task<Order> GetOrderByIdAsync(int orderId);

        //Receive Order
        public Task<Receive_Order[]> GetReceivedOrdersAsync();
        public Task<Receive_Order> GetReceivedOrdersByIdAsync(int receiveOrderId);

        //Supplier Order Product
        public Task<Supplier_Order_Product[]> GetSupplierOrderProductAsync();
        public Task<Supplier_Order_Product> GetSupplierOrderProductByIdAsync(int orderId, int productId);
        public Task<Supplier_Order_Product[]> FilterProductsByOrderIdAsync(int orderId);

        ///////////////////////////////////////////////////////SUPPLIER REPOSITORY END///////////////////////////////////////////////////////////////////////////////////     


        ///////////////////////////////////////////////////////EVENT REPOSITORY///////////////////////////////////////////////////////////////////////////////////////////

        //Event Type
        public Task<Event_Types[]> GetEventTypesAsync();    
        public Task<Event_Types> GetEventTypesByIdAsync(int eventTypesId);

        //Event 
        public Task<Event_Booking[]> GetEventBookingsAsync();
        public Task<Event_Booking> GetEventBookingByIdAsync(int eventBookingId);

        ///////////////////////////////////////////////////////EVENT REPOSITORY END///////////////////////////////////////////////////////////////////////////////////////
    }
}
