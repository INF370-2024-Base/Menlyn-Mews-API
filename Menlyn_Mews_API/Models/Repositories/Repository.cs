using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        ///////////////////////////////////////////////////////CLIENT REPOSITORY END////////////////////////////////////////////////////////////////////////////////////
    }

}
