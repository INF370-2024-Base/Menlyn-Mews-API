using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;

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
            IQueryable<Product> query = _context.Products.Where(p => p.ProductId == productId).Include(p => p.Inventory).Include(p => p.ProductType).Include(p => p.ProductCategory).Include(p => p.Price);
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
            IQueryable<Inventory> query =  _context.Inventories.Include(i => i.InventoryCategory).Include(i => i.InventoryType).Include(i => i.Room);
            return await query.ToArrayAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            IQueryable<Inventory> query = _context.Inventories.Where(i => i.InventoryId == inventoryId).Include(i => i.InventoryCategory).Include(i => i.InventoryType).Include(i => i.Room);
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
            IQueryable<Inspection_Item> query = _context.Inspection_Items.Include(ii => ii.Inventory);
            return await query.ToArrayAsync();
        }

        public async Task<Inspection_Item> GetInspectionItemsByIdAsync(int inspectionItemId)
        {
            IQueryable<Inspection_Item> query = _context.Inspection_Items.Where(ii => ii.InspectionItemId == inspectionItemId).Include(ii => ii.Inventory);
            return await query.FirstOrDefaultAsync();
        }

        //Write-Off
        public async Task<Write_Off[]> GetWrite_OffsAsync()
        {
            IQueryable<Write_Off> query = _context.Write_Offs.Include(wo => wo.Inventory).Include(wo => wo.Employee_Shift.Employee).Include(wo => wo.Employee_Shift.Shift);
            return await query.ToArrayAsync();
        }

        public async Task<Write_Off> GetWrite_OffByIdAsync(int writeOffId)
        {
            IQueryable<Write_Off> query = _context.Write_Offs.Where(wo => wo.WriteOffId == writeOffId).Include(wo => wo.Inventory).Include(wo => wo.Employee_Shift.Employee).Include(wo => wo.Employee_Shift.Shift);
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

        //Employee
        public async Task<Employee[]> GetEmployeesAsync()
        {
            IQueryable<Employee> query = _context.Employees.Include(e => e.Employee_Type).Include(e => e.Position).Include(r => r.Rates);
            return await query.ToArrayAsync();
        }
        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            IQueryable<Employee> query = _context.Employees.Where(e => e.EmployeeId == employeeId).Include(e => e.Employee_Type).Include(e => e.Position).Include(r => r.Rates);
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
            IQueryable<Employee_Shift> query = _context.Employee_Shifts.Include(es => es.Employee).Include(es => es.Employee.Rates).Include(es => es.Shift);
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

        ///////////////////////////////////////////////////////BOOKING REPOSITORY END////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////CLIENT REPOSITORY////////////////////////////////////////////////////////////////////////////////////////

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
            IQueryable<Booking_Review> query = _context.Booking_Reviews.Include(br => br.Client);
            return await query.ToArrayAsync();
        }
        public async Task<Booking_Review> GetBookingReviewByIdAsync(int bookingReviewId)
        {
            IQueryable<Booking_Review> query = _context.Booking_Reviews.Where(br => br.BookingReviewId == bookingReviewId).Include(er => er.Client);
            return await query.FirstOrDefaultAsync();
        }

        //Client
        public async Task<Client[]> GetClientsAsync()
        {
            IQueryable<Client> query = _context.Clients;
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
            IQueryable<Order> query = _context.Orders.Include(o => o.Suppliers).Include(o => o.Employee_Shift.Employee).Include(o => o.Employee_Shift.Shift);
            return await query.ToArrayAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            IQueryable<Order> query = _context.Orders.Where(o => o.OrderId == orderId).Include(o => o.Suppliers).Include(o => o.Employee_Shift.Employee).Include(o => o.Employee_Shift.Shift);
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
            IQueryable<Event_Booking> query = _context.Event_Bookings.Include(eb => eb.Event_Types).Include(eb => eb.Client).Include(eb => eb.Employee_Shift.Employee).Include(eb => eb.Employee_Shift.Shift);
            return await query.ToArrayAsync();  
        }
        public async Task<Event_Booking> GetEventBookingByIdAsync(int eventBookingId)
        {
            IQueryable<Event_Booking> query = _context.Event_Bookings.Where(eb => eb.EventId == eventBookingId).Include(eb => eb.Event_Types).Include(eb => eb.Client).Include(eb => eb.Employee_Shift.Employee).Include(eb => eb.Employee_Shift.Shift);
            return await query.FirstOrDefaultAsync();
        }


        ///////////////////////////////////////////////////////EVENT REPOSITORY END///////////////////////////////////////////////////////////////////////////////////////
    }

}
