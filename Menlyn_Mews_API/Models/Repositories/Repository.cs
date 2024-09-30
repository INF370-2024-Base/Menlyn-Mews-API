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

        //Floating Tables
        public async Task<VAT[]> GetVATAsync()
        {
            IQueryable<VAT> query = _context.VAT;
            return await query.ToArrayAsync();
        }

        public async Task<VAT> GetVATByIdAsync(int vatId)
        {
            IQueryable<VAT> query = _context.VAT.Where(v => v.VATId == vatId);
            return await query.FirstOrDefaultAsync();
        }
        //

        ///////////////////////////////////////////////////////PRODUCT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////
        //Products
        public async Task<Product[]> GetProductsAsync()
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Inventory).Include(p => p.ProductType).Include(p => p.Price).Include(p => p.ProductType.ProductCategory);

            return await query.ToArrayAsync();
        }

        public async Task<Product[]> GetProductsReportAsync()
        {
            IQueryable<Product> query = _context.Products;

            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            IQueryable<Product> query = _context.Products.Where(p => p.ProductId == productId).Include(p => p.Inventory).Include(p => p.ProductType).Include(p => p.Price).Include(p => p.ProductType.ProductCategory);
            return await query.FirstOrDefaultAsync();
        }

        //Product Type
        public async Task<Product_Type[]> GetProductTypesAsync()
        {
            IQueryable<Product_Type> query = _context.Product_Types.Include(p => p.ProductCategory);
            return await query.ToArrayAsync();
        }

        public async Task<Product_Type> GetProductTypeByIdAsync(int productTypeId)
        {
            IQueryable<Product_Type> query = _context.Product_Types.Where(pt => pt.ProductTypeId == productTypeId).Include(p => p.ProductCategory);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product_Type[]> GetProductTypesByCategoryAsync(int productCategoryId)
        {
            IQueryable<Product_Type> query = _context.Product_Types.Where(p => p.ProductCategoryId == productCategoryId).Include(p => p.ProductCategory);
            return await query.ToArrayAsync();
        }

        //Product Category
        public async Task<Product_Category[]> GetProductCategoriesAsync()
        {
            IQueryable<Product_Category> query = _context.Product_Categories;
            return await query.ToArrayAsync();
        }

        public async Task<Product_Category> GetProductCategoryByIdAsync(int categoryId)
        {
            IQueryable<Product_Category> query = _context.Product_Categories.Where(pc => pc.ProductCategoryId == categoryId);
            return await query.FirstOrDefaultAsync();
        }

        //Price
        public async Task<Price[]> GetPricesAsync()
        {
            IQueryable<Price> query = _context.Prices;
            return await query.ToArrayAsync();
        }

        public async Task<Price> GetPriceByIdAsync(int priceId)
        {
            IQueryable<Price> query = _context.Prices.Where(p => p.PriceId == priceId);
            return await query.FirstOrDefaultAsync();
        }

        ///////////////////////////////////////////////////////PRODUCT REPOSITORY END/////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////INVENTORY REPOSITORY///////////////////////////////////////////////////////////////////////////////////////
        //Inventory
        public async Task<Inventory[]> GetInventoriesAsync()
        {
            IQueryable<Inventory> query = _context.Inventories.Include(i => i.InventoryCategory).Include(i => i.InventoryType);
            return await query.ToArrayAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            IQueryable<Inventory> query = _context.Inventories.Where(i => i.InventoryId == inventoryId).Include(i => i.InventoryCategory).Include(i => i.InventoryType);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Inventory> GetInventoryByProductNameAsync(string productName)
        {
            IQueryable<Inventory> query = _context.Inventories.Where(i => i.Inventory_Name == productName).Include(i => i.InventoryCategory).Include(i => i.InventoryType);
            return await query.FirstOrDefaultAsync();
        }

        //Inventory Type
        public async Task<Inventory_Type[]> GetInventoryTypesAsync()
        {
            IQueryable<Inventory_Type> query = _context.Inventory_Types;
            return await query.ToArrayAsync();
        }

        public async Task<Inventory_Type> GetInventoryTypesByIdAsync(int inventoryTypeId)
        {
            IQueryable<Inventory_Type> query = _context.Inventory_Types.Where(it => it.InventoryTypeId == inventoryTypeId);
            return await query.FirstOrDefaultAsync();
        }

        //Inventory Category
        public async Task<Inventory_Category[]> GetInventoryCategoriesAsync()
        {
            IQueryable<Inventory_Category> query = _context.Inventory_Categories;
            return await query.ToArrayAsync();
        }

        public async Task<Inventory_Category> GetInventoryCategoriesByIdAsync(int inventoryCategoryId)
        {
            IQueryable<Inventory_Category> query = _context.Inventory_Categories.Where(ic => ic.InventoryCategoryId == inventoryCategoryId);
            return await query.FirstOrDefaultAsync();
        }

        //Inspection Item
        public async Task<Inspection_Item[]> GetInspectionItemsAsync()
        {
            IQueryable<Inspection_Item> query = _context.Inspection_Items.Include(ii => ii.Room_Booking).Include(ii => ii.Employee).Include(ii => ii.Room_Booking.Rooms).Include(ii => ii.Room_Booking.Clients);
            return await query.ToArrayAsync();
        }

        public async Task<Inspection_Item> GetInspectionItemsByIdAsync(int inspectionItemId)
        {
            IQueryable<Inspection_Item> query = _context.Inspection_Items.Where(ii => ii.InspectionItemId == inspectionItemId).Include(ii => ii.Room_Booking).Include(ii => ii.Employee).Include(ii => ii.Room_Booking.Rooms).Include(ii => ii.Room_Booking.Clients);;
            return await query.FirstOrDefaultAsync();
        }

        //Write-Off
        public async Task<Write_Off[]> GetWrite_OffsAsync()
        {
            IQueryable<Write_Off> query = _context.Write_Offs.Include(wo => wo.Room_Inventory.Inventory).Include(wo => wo.Employee).Include(wo => wo.Inspection_Item).Include(wo => wo.RoomBooking).Include(wo => wo.RoomBooking.Rooms).Include(wo => wo.RoomBooking.Clients);
            return await query.ToArrayAsync();
        }

        public async Task<Write_Off> GetWrite_OffByIdAsync(int writeOffId)
        {
            IQueryable<Write_Off> query = _context.Write_Offs.Where(wo => wo.WriteOffId == writeOffId).Include(wo => wo.Room_Inventory.Inventory).Include(wo => wo.Room_Inventory.Room).Include(wo => wo.Employee).Include(wo => wo.Inspection_Item).Include(wo => wo.RoomBooking).Include(wo => wo.RoomBooking.Rooms).Include(wo => wo.RoomBooking.Clients);
            return await query.FirstOrDefaultAsync();
        }

        //Room_Inventory
        public async Task<Room_Inventory[]> GetRoomInventoriesAsync()
        {
            IQueryable<Room_Inventory> query = _context.Room_Inventory.Include(ri => ri.Room).Include(ri => ri.Inventory);
            return await query.ToArrayAsync();
        }

        public async Task<Room_Inventory> GetRoomInventoryByIdAsync(int roomId, int inventoryId)
        {
            IQueryable<Room_Inventory> query = _context.Room_Inventory.Where(ri => ri.RoomId == roomId && ri.InventoryId == inventoryId).Include(ri => ri.Room).Include(ri => ri.Inventory);
            return await query.FirstOrDefaultAsync();
        }

        //Stock Take
        public async Task<Stock_Take[]> GetStockTakesAsync()
        {
            IQueryable<Stock_Take> query = _context.Stock_Takes.Include(st => st.Employee_Shift.Employee).Include(st => st.Employee_Shift.Shift).Include(st => st.Inventory);
            return await query.ToArrayAsync();
        }

        public async Task<Stock_Take> GetStockTakeByIdAsync(int stockTakeId)
        {
            IQueryable<Stock_Take> query = _context.Stock_Takes.Where(st => st.StockTakeId == stockTakeId).Include(st => st.Employee_Shift.Employee).Include(st => st.Employee_Shift.Shift).Include(st => st.Inventory);
            return await query.FirstOrDefaultAsync();
        }

        //Filter Inventory Name
        public async Task<Product[]> FilterInventoryNameAsync(string inventoryName)
        {
            IQueryable<Product> query = _context.Products.Where(i => i.Product_Name != inventoryName);
            return await query.ToArrayAsync();
        }

        ///////////////////////////////////////////////////////INVENTORY REPOSITORY END///////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////EMPLOYEE REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

        //Position
        public async Task<Position[]> GetPositionsAsync()
        {
            IQueryable<Position> query = _context.Positions;
            return await query.ToArrayAsync();
        }

        public async Task<Position> GetPositionByIdAsync(int positionId)
        {
            IQueryable<Position> query = _context.Positions.Where(p => p.PositionId == positionId);
            return await query.FirstOrDefaultAsync();
        }

        //Employee Type
        public async Task<Employee_Type[]> GetEmployeeTypesAsync()
        {
            IQueryable<Employee_Type> query = _context.Employee_Types;
            return await query.ToArrayAsync();
        }

        public async Task<Employee_Type> GetEmployeeTypeByIdAsync(int employeeTypeId)
        {
            IQueryable<Employee_Type> query = _context.Employee_Types.Where(et => et.EmployeeTypeId == employeeTypeId);
            return await query.FirstOrDefaultAsync();
        }

        //Shift
        public async Task<Shift[]> GetShiftsAsync()
        {
            IQueryable<Shift> query = _context.Shifts;
            return await query.ToArrayAsync();
        }
        public async Task<Shift> GetShiftByIdAsync(int shiftId)
        {
            IQueryable<Shift> query = _context.Shifts.Where(s => s.ShiftId == shiftId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Shift[]> GetShiftByDateAsync(DateTime date)
        {
            IQueryable<Shift> query = _context.Shifts.Where(s => s.Shift_Date == date);
            return await query.ToArrayAsync();
        }

        //Employee
        public async Task<Employee[]> GetEmployeesAsync()
        {
            IQueryable<Employee> query = _context.Employees.Include(e => e.Employee_Type).Include(e => e.Position).Include(r => r.Rates).Include(a => a.ApplicationUser);
            return await query.ToArrayAsync();
        }
        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            IQueryable<Employee> query = _context.Employees.Where(e => e.EmployeeId == employeeId).Include(e => e.Employee_Type).Include(e => e.Position).Include(r => r.Rates).Include(a => a.ApplicationUser);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Employee> GetEmployeeByAppUserIdAsync(string appUserId)
        {
            IQueryable<Employee> query = _context.Employees.Where(e => e.ApplicationUserId == appUserId).Include(e => e.Employee_Type).Include(e => e.Position).Include(r => r.Rates);
            return await query.FirstOrDefaultAsync();
        }

        //Employee_Shift
        public async Task<Employee_Shift[]> GetEmployeeShiftsAsync()
        {
            IQueryable<Employee_Shift> query = _context.Employee_Shifts.Include(es => es.Employee).Include(es => es.Shift);
            return await query.ToArrayAsync();
        }

        public async Task<Employee_Shift[]> GetEmployeeShiftWithRateAsync()
        {
            IQueryable<Employee_Shift> query = _context.Employee_Shifts.Include(es => es.Employee).Include(es => es.Employee.Rates).Include(es => es.Shift).Include(es => es.Employee.Employee_Type);
            return await query.ToArrayAsync();
        }

        public async Task<Employee_Shift> GetEmployeeShiftByIdAsync(int employeeId, int shiftId)
        {
            IQueryable<Employee_Shift> query = _context.Employee_Shifts.Where(es => es.Employee.EmployeeId == employeeId && es.Shift.ShiftId == shiftId).Include(es => es.Employee).Include(es => es.Shift);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Employee_Shift> GetEmployeeShiftByIdEmployeeAsync(int employeeId)
        {
            IQueryable<Employee_Shift> query = _context.Employee_Shifts.Where(es => es.Employee.EmployeeId == employeeId).Include(es => es.Employee).Include(es => es.Shift);
            return await query.FirstOrDefaultAsync();
        }

        //Rates
        public async Task<Rates[]> GetRatesAsync()
        {
            IQueryable<Rates> query = _context.Rates;
            return await query.ToArrayAsync();
        }
        public async Task<Rates> GetRatesByIdAsync(int ratesId)
        {
            IQueryable<Rates> query = _context.Rates.Where(r => r.RateId == ratesId);
            return await query.FirstOrDefaultAsync();
        }

        ///////////////////////////////////////////////////////EMPLOYEE REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////BOOKING REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

        //Booking Package
        public async Task<Booking_Package[]> GetBookingPackagesAsync()
        {
            IQueryable<Booking_Package> query = _context.Booking_Packages;
            return await query.ToArrayAsync();
        }

        public async Task<Booking_Package> GetBookingPackageByIdAsync(int bookingPackageId)
        {
            IQueryable<Booking_Package> query = _context.Booking_Packages.Where(bp => bp.BookingPackageId == bookingPackageId);
            return await query.FirstOrDefaultAsync();
        }

        //Discount
        public async Task<Discount[]> GetDiscountsAsync()
        {
            IQueryable<Discount> query = _context.Discount;
            return await query.ToArrayAsync();
        }

        public async Task<Discount> GetDiscountByIdAsync(int discountId)
        {
            IQueryable<Discount> query = _context.Discount.Where(d => d.DiscountId == discountId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Discount> FindDiscountCodeAsync(string code)
        {
            IQueryable<Discount> query = _context.Discount
                .Where(d => EF.Functions.Collate(d.Discount_Code, "SQL_Latin1_General_CP1_CS_AS") == code);
            return await query.FirstOrDefaultAsync();
        }


        //Room Type
        public async Task<Room_Type[]> GetRoomTypesAsync()
        {
            IQueryable<Room_Type> query = _context.Room_Types;
            return await query.ToArrayAsync();
        }

        public async Task<Room_Type> GetRoomTypeByIdAsync(int roomTypeId)
        {
            IQueryable<Room_Type> query = _context.Room_Types.Where(rt => rt.RoomTypeId == roomTypeId);
            return await query.FirstOrDefaultAsync();
        }

        //Rooms
        public async Task<Room[]> GetRoomsAsync()
        {
            IQueryable<Room> query = _context.Rooms.Include(r => r.Room_Type);
            return await query.ToArrayAsync();
        }
        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            IQueryable<Room> query = _context.Rooms.Where(r => r.RoomId == roomId).Include(r => r.Room_Type);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Room_Inventory[]> FilterInventoriesByRoomIdAsync(int roomId)
        {
            IQueryable<Room_Inventory> query = _context.Room_Inventory.Where(ri => ri.RoomId == roomId).Include(ri => ri.Inventory).Include(ri => ri.Room);
            return await query.ToArrayAsync();  
        }

        //Room Booking
        public async Task<Room_Booking[]> GetRoomBookingsAsync()
        {
            IQueryable<Room_Booking> query = _context.Room_Bookings.Include(rb => rb.Clients).Include(rb => rb.Rooms).Include(rb => rb.Booking_Package).Include(rb => rb.Discount);
            return await query.ToArrayAsync();
        }
        public async Task<Room_Booking> GetRoomBookingByIdAsync(int bookingId)
        {
            IQueryable<Room_Booking> query = _context.Room_Bookings.Where(rb => rb.RoomBookingId == bookingId).Include(rb => rb.Clients).Include(rb => rb.Rooms).Include(rb => rb.Booking_Package).Include(rb => rb.Discount);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Room_Booking[]> GetRoomBookingByClientIdAsync(int clientId)
        {
            IQueryable<Room_Booking> query = _context.Room_Bookings.Where(rb => rb.ClientId == clientId).Include(rb => rb.Clients).Include(rb => rb.Rooms).Include(rb => rb.Booking_Package).Include(rb => rb.Discount).Include(rb => rb.Booking_Review);
            return await query.ToArrayAsync();
        }

        public async Task<Room_Booking[]> GetBookedRooms(int roomId)
        {
            IQueryable<Room_Booking> query = _context.Room_Bookings.Where(rb => rb.RoomId == roomId && rb.Booking_Status == "Booked").Include(rb => rb.Clients).Include(rb => rb.Rooms).Include(rb => rb.Booking_Package).Include(rb => rb.Discount);
            return await query.ToArrayAsync();
        }

        //Payments

        public async Task<Payment[]> GetPaymentsAsync()
        {
            IQueryable<Payment> query = _context.Payment.Include(p => p.Client).Include(p => p.Payment_Type);
            return await query.ToArrayAsync();
        }
        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            IQueryable<Payment> query = _context.Payment.Where(p => p.PaymentId == paymentId).Include(p => p.Client).Include(p => p.Payment_Type);
            return await query.FirstOrDefaultAsync();
        }

        //Paymemt Types 
        public async Task<Payment_Type[]> GetPaymentTypesAsync()
        {
            IQueryable<Payment_Type> query = _context.Payment_Types;
            return await query.ToArrayAsync();
        }
        public async Task<Payment_Type> GetPaymentTypeByIdAsync(int paymentTypeId)
        {
            IQueryable<Payment_Type> query = _context.Payment_Types.Where(pt => pt.PaymentTypeId == paymentTypeId);
            return await query.FirstOrDefaultAsync();
        }

        ///////////////////////////////////////////////////////BOOKING REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////CLIENT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

        //Get Client By AppUserId
        public async Task<Client> GetClientByAppUserIdAsync(string appUserId)
        {
            IQueryable<Client> query = _context.Clients.Where(c => c.ApplicationUserId == appUserId).Include(c => c.ApplicationUser);
            return await query.FirstOrDefaultAsync();
        }

        //Event Review
        public async Task<Event_Review[]> GetEventReviewsAsync()
        {
            IQueryable<Event_Review> query = _context.Event_Reviews.Include(er => er.Client);
            return await query.ToArrayAsync();  
        }
        public async Task<Event_Review> GetEventReviewByIdAsync(int eventReviewId)
        {
            IQueryable<Event_Review> query = _context.Event_Reviews.Where(er => er.EventReviewId == eventReviewId).Include(er => er.Client);
            return await query.FirstOrDefaultAsync();
        }

        //Booking Review
        public async Task<Booking_Review[]> GetBookingReviewsAsync()
        {
            IQueryable<Booking_Review> query = _context.Booking_Reviews.Include(br => br.Client).Include(br => br.Room).Include(br => br.Room_Booking);
            return await query.ToArrayAsync();
        }
        public async Task<Booking_Review> GetBookingReviewByIdAsync(int bookingReviewId)
        {
            IQueryable<Booking_Review> query = _context.Booking_Reviews.Where(br => br.BookingReviewId == bookingReviewId).Include(er => er.Client).Include(br => br.Room_Booking);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Booking_Review[]> GetReviewsByRoomIdAsync(int roomId)
        {
            IQueryable<Booking_Review> query = _context.Booking_Reviews.Where(br => br.RoomId == roomId).Include(br => br.Room).Include(br => br.Client).Include(br => br.Client.ApplicationUser).Include(br => br.Room_Booking);
            return await query.ToArrayAsync();
        }

        //Client
        public async Task<Client[]> GetClientsAsync()
        {
            IQueryable<Client> query = _context.Clients.Include(c => c.ApplicationUser);
            return await query.ToArrayAsync();  
        }
        public async Task<Client> GetClientByIdAsync(int clientId)
        {
            IQueryable<Client> query = _context.Clients.Where(C => C.ClientId == clientId);
            return await query.FirstOrDefaultAsync();
        }

        //Complaint Type
        public async Task<Complaint_Type[]> GetComplaintTypesAsync()
        {
            IQueryable<Complaint_Type> query = _context.Complaint_Types;
            return await query.ToArrayAsync();
        }
        public async Task<Complaint_Type> GetComplaintTypeByIdAsync(int complaintTypeId)
        {
            IQueryable<Complaint_Type> query = _context.Complaint_Types.Where(ct => ct.ComplaintTypeId == complaintTypeId);
            return await query.FirstOrDefaultAsync();
        }        

        //Complaint
        public async Task<Complaint[]> GetComplaintsAsync()
        {
            IQueryable<Complaint> query = _context.Complaints.Include(c => c.Employee).Include(c => c.Client).Include(c => c.Complaint_Type);
            return await query.ToArrayAsync();  
        }
        public async Task<Complaint> GetComplaintByIdAsync(int complaintId)
        {
            IQueryable<Complaint> query = _context.Complaints.Where(c => c.ComplaintId == complaintId).Include(c => c.Employee).Include(c => c.Client).Include(c => c.Complaint_Type);
            return await query.FirstOrDefaultAsync();
        }

        //Client Discount
        public async Task<Client_Discount[]> GetClientDiscountsAsync()
        {
            IQueryable<Client_Discount> query = _context.Client_Discounts.Include(cd => cd.Client).Include(cd => cd.Discount);
            return await query.ToArrayAsync();
        }

        public async Task<Client_Discount> GetClientDiscountByIdAsync(int discountId, int clientId)
        {
            IQueryable<Client_Discount> query = _context.Client_Discounts.Where(cd => cd.DiscountId == discountId && cd.ClientId == clientId).Include(cd => cd.Client).Include(cd => cd.Discount);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Client_Discount[]> GetUsedClientDiscountsAsync(int clientId)
        {
            IQueryable<Client_Discount> query = _context.Client_Discounts.Where(cd => cd.ClientId == clientId).Include(cd => cd.Client).Include(cd => cd.Discount);
            return await query.ToArrayAsync();
        }

        ///////////////////////////////////////////////////////CLIENT REPOSITORY END////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////SUPPLIER REPOSITORY///////////////////////////////////////////////////////////////////////////////////////

        //Supplier Type
        public async Task<Supplier_Type[]> GetSupplierTypesAsync()
        {
            IQueryable<Supplier_Type> query = _context.Supplier_Types;
            return await query.ToArrayAsync();  
        }
        public async Task<Supplier_Type> GetSupplierTypeByIdAsync(int supplierTypeId)
        {
            IQueryable<Supplier_Type> query = _context.Supplier_Types.Where(st => st.SupplierTypeId == supplierTypeId);
            return await query.FirstOrDefaultAsync();
        }

        //Supplier
        public async Task<Supplier[]> GetSuppliersAsync()
        {
            IQueryable<Supplier> query = _context.Suppliers.Include(s => s.Supplier_Type);
            return await query.ToArrayAsync();
        }
        public async Task<Supplier> GetSupplierByIdAsync(int supplierId)
        {
            IQueryable<Supplier> query = _context.Suppliers.Where(s => s.SupplierId == supplierId).Include(s => s.Supplier_Type);
            return await query.FirstOrDefaultAsync();
        }

        //Order
        public async Task<Order[]> GetOrdersAsync()
        {
            IQueryable<Order> query = _context.Orders.Include(o => o.Suppliers).Include(o => o.Employee);
            return await query.ToArrayAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            IQueryable<Order> query = _context.Orders.Where(o => o.OrderId == orderId).Include(o => o.Suppliers).Include(o => o.Employee);
            return await query.FirstOrDefaultAsync();
        }

        //Receive Order
        public async Task<Receive_Order[]> GetReceivedOrdersAsync()
        {
            IQueryable<Receive_Order> query = _context.Receive_Orders;
            return await query.ToArrayAsync();  
        }
        public async Task<Receive_Order> GetReceivedOrdersByIdAsync(int receiveOrderId)
        {
            IQueryable<Receive_Order> query = _context.Receive_Orders.Where(ro => ro.ReceieveOrderId == receiveOrderId);
            return await query.FirstOrDefaultAsync();
        }

        //Supplier Order Product
        public async Task<Supplier_Order_Product[]> GetSupplierOrderProductAsync()
        {
            IQueryable<Supplier_Order_Product> query = _context.Supplier_Order_Products.Include(op => op.Order).Include(op => op.Product).Include(op => op.Product.Price).Include(op => op.Receive_Order);
            return await query.ToArrayAsync();
        }

        public async Task<Supplier_Order_Product> GetSupplierOrderProductByIdAsync(int orderId, int productId)
        {
            IQueryable<Supplier_Order_Product> query = _context.Supplier_Order_Products.Where(op => op.OrderId == orderId && op.ProductId == productId).Include(op => op.Order).Include(op => op.Order).Include(op => op.Product.Price).Include(op => op.Receive_Order);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Supplier_Order_Product[]> FilterProductsByOrderIdAsync(int orderId)
        {
            IQueryable<Supplier_Order_Product> query = _context.Supplier_Order_Products.Where(op => op.OrderId == orderId).Include(op => op.Product).Include(op => op.Order);
            return await query.ToArrayAsync();
        }

        ///////////////////////////////////////////////////////SUPPLIER REPOSITORY END///////////////////////////////////////////////////////////////////////////////////    


        ///////////////////////////////////////////////////////EVENT REPOSITORY///////////////////////////////////////////////////////////////////////////////////////////

        //Event Type
        public async Task<Event_Types[]> GetEventTypesAsync()
        {
            IQueryable<Event_Types> query = _context.Event_Types;
            return await query.ToArrayAsync();
        }
        public async Task<Event_Types> GetEventTypesByIdAsync(int eventTypesId)
        {
            IQueryable<Event_Types> query = _context.Event_Types.Where(et => et.EventTypeId == eventTypesId);
            return await query.FirstOrDefaultAsync();
        }

        //Event 
        public async Task<Event_Booking[]> GetEventBookingsAsync()
        {
            IQueryable<Event_Booking> query = _context.Event_Bookings.Include(eb => eb.Event_Types).Include(eb => eb.Client).Include(eb => eb.Employee);
            return await query.ToArrayAsync();  
        }
        public async Task<Event_Booking> GetEventBookingByIdAsync(int eventBookingId)
        {
            IQueryable<Event_Booking> query = _context.Event_Bookings.Where(eb => eb.EventId == eventBookingId).Include(eb => eb.Event_Types).Include(eb => eb.Client).Include(eb => eb.Employee);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Event_Booking[]> GetEventBookingByClientIdAsync(int clientId)
        {
            IQueryable<Event_Booking> query = _context.Event_Bookings.Where(eb => eb.ClientId == clientId).Include(eb => eb.Event_Types);
            return await query.ToArrayAsync();
        }


        ///////////////////////////////////////////////////////EVENT REPOSITORY END///////////////////////////////////////////////////////////////////////////////////////
    }

}
