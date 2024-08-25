using Microsoft.EntityFrameworkCore; // added 13 / 04 / 2024
using Menlyn_Mews_API.Models.Domain; //added 24 / 05 / 2024
using Microsoft.AspNetCore.Identity; //added 24 / 05 / 2024
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // added 13 / 04 / 2024

namespace Menlyn_Mews_API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Booking_Review> Booking_Reviews { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Employee_Type> Employee_Types { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Inventory_Category> Inventory_Categories { get; set; }

        public DbSet<Inventory_Type> Inventory_Types { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Room_Type> Room_Types { get; set; }

        public DbSet<Room_Booking> Room_Bookings { get; set; }

        //Added 06/06/2024
        public DbSet<Supplier_Order_Product> Supplier_Order_Products { get; set; }
        
        public DbSet<Supplier> Suppliers { get; set; }
        
        public DbSet<Supplier_Type> Supplier_Types { get; set; }   

        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Product> Products { get; set; }

        public DbSet<Price> Prices { get; set; }

        public DbSet<Product_Type> Product_Types { get; set; }

        public DbSet<Product_Category> Product_Categories { get; set; }
        
        public DbSet<Receive_Order> Receive_Orders { get; set; }

        public DbSet<Employee_Shift> Employee_Shifts { get; set; }  

        public DbSet<Position> Positions { get; set; }

        public DbSet<Inspection_Item> Inspection_Items { get; set; }

        public DbSet<Stock_Take> Stock_Takes { get; set; }

        public DbSet<Write_Off> Write_Offs { get; set; }

        //Added 01/07/2024
        public DbSet<VAT> VAT { get; set; }

        public DbSet<Event_Review> Event_Reviews { get; set; }

        public DbSet<Booking_Package> Booking_Packages { get; set; }    

        public DbSet<Discount> Discount { get; set; }   

        public DbSet<Referrals> Referrals { get; set; }

        public DbSet<Event_Types> Event_Types { get; set; }

        public DbSet<Event_Booking> Event_Bookings { get; set; }

        public DbSet<Audit_Log> Audit_Logs { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<Payment_Type> Payment_Types { get; set; }

        public DbSet<Complaint_Type> Complaint_Types { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<Shift> Shifts { get; set; }
        
        public DbSet<Rates> Rates { get; set; }

        public DbSet<Audit_Log> AuditLogs { get; set; }

        public DbSet<Room_Inventory> Room_Inventory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Audit_Log>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.User_Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Controller_Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Action_Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Details).HasMaxLength(2000);
            });

            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Employee)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<Employee>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                .HasOne(r => r.Rates)
                .WithMany(e => e.Employee)
                .HasForeignKey(r => r.RateId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Write_Off>()
                .HasOne(wo => wo.Client)
                .WithMany(c => c.Write_Off)
                .HasForeignKey(wo => wo.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Room_Type)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Employee_Type)
                .WithMany(et => et.Employee)
                .HasForeignKey(e => e.EmployeeTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany(p => p.Employee) 
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee_Shift>()
                .HasKey(es => new
                {
                    es.EmployeeId,
                    es.ShiftId,
                });

            modelBuilder.Entity<Room_Inventory>()
                .HasKey(ri => new
                {
                    ri.RoomId,  
                    ri.InventoryId,
                });



            modelBuilder.Entity<Room_Inventory>()
                .HasOne(ri => ri.Room)
                .WithMany(ri => ri.Room_Inventory)
                .HasForeignKey(ri => ri.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room_Inventory>()
                .HasOne(ri => ri.Inventory)
                .WithMany(ri => ri.Room_Inventory)
                .HasForeignKey(ri => ri.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier_Order_Product>()
                .HasKey(sop => new
                {
                    sop.OrderId,
                    sop.ProductId,
                });

            modelBuilder.Entity<Employee_Shift>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.Employee_Shift)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee_Shift>()
                .HasOne(es => es.Shift)
                .WithMany(s => s.Employee_Shift)
                .HasForeignKey(es => es.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stock_Take>()
                .HasOne(i => i.Inventory)
                .WithMany(st => st.Stock_Take)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee_Shift>()
                .HasMany(es => es.Stock_Take)
                .WithOne(st => st.Employee_Shift)
                .HasForeignKey(st => new
                {
                    st.EmployeeId,
                    st.ShiftId
                })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee_Shift>()
                .HasMany(es => es.Inspection_Item)
                .WithOne(ii => ii.Employee_Shift)
                .HasForeignKey(es => new
                {
                    es.EmployeeId,
                    es.ShiftId  
                })
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room_Inventory>()
                .HasMany(wo => wo.Write_Off)
                .WithOne(ri => ri.Room_Inventory)
                .HasForeignKey(ri => new
                {
                    ri.RoomId,
                    ri.InventoryId
                })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee_Shift>()
                .HasMany(es => es.Write_Off)
                .WithOne(wo => wo.Employee_Shift)
                .HasForeignKey(wo => new
                {
                    wo.EmployeeId,
                    wo.ShiftId
                })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee_Shift>()
                .HasMany(es => es.Order)
                .WithOne(o => o.Employee_Shift)
                .HasForeignKey(o => new
                {
                    o.EmployeeId,
                    o.ShiftId
                })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier_Type>()
                .HasMany(st => st.Suppliers)
                .WithOne(s => s.Supplier_Type)
                .HasForeignKey(st => st.SupplierTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inspection_Item>()
                .HasMany(ii => ii.Write_Off)
                .WithOne(wo => wo.Inspection_Item)
                .HasForeignKey(ii => ii.InspectionItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Orders)
                .WithOne(o => o.Suppliers)
                .HasForeignKey(s => s.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier_Order_Product>()
                .HasOne(sop => sop.Order)
                .WithMany(o => o.Supplier_Order_Product)
                .HasForeignKey(sop => sop.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier_Order_Product>()
                .HasOne(sop => sop.Product)
                .WithMany(o => o.Supplier_Order_Product)
                .HasForeignKey(sop => sop.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Receive_Order>()
                .HasMany(ro => ro.Supplier_Order_Product)
                .WithOne(sop => sop.Receive_Order)
                .HasForeignKey(ro => ro.ReceiveOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event_Types>()
                .HasMany(eb => eb.Event_Booking)
                .WithOne(et => et.Event_Types)
                .HasForeignKey(eb => eb.EventTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee_Shift>()
                .HasMany(es => es.Event_Booking)
                .WithOne(wo => wo.Employee_Shift)
                .HasForeignKey(wo => new
                {
                    wo.EmployeeId,
                    wo.ShiftId
                })
                .OnDelete(DeleteBehavior.Cascade);

            //ROOM BOOKING
            modelBuilder.Entity<Booking_Package>()
                .HasMany(rb => rb.Room_Booking)
                .WithOne(bp => bp.Booking_Package)
                .HasForeignKey(bp => bp.BookingPackageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(rb => rb.Room_Bookings)
                .WithOne(bp => bp.Rooms)
                .HasForeignKey(bp => bp.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Discount>()
                .HasMany(rb => rb.Room_Booking)
                .WithOne(bp => bp.Discount)
                .HasForeignKey(bp => bp.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(rb => rb.Room_Bookings)
                .WithOne(bp => bp.Clients)
                .HasForeignKey(bp => bp.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment_Type>()
                .HasMany(p => p.Payments)
                .WithOne(pt => pt.Payment_Type)
                .HasForeignKey(fk => fk.PaymentTypeId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Complaint_Type>()
                .HasMany(c => c.Complaint)
                .WithOne(ct => ct.Complaint_Type)
                .HasForeignKey(fk => fk.ComplaintTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(c => c.Complaint)
                .WithOne(ct => ct.Employee)
                .HasForeignKey(fk => fk.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Complaint)
                .WithOne(ct => ct.Client)
                .HasForeignKey(fk => fk.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Client)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<Client>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(br => br.Booking_Review)
                .WithOne(r => r.Room)
                .HasForeignKey(br => br.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room_Booking>()
                .HasOne(rb => rb.Booking_Review)
                .WithOne(br => br.Room_Booking)
                .HasForeignKey<Booking_Review>(br => br.RoomBookingId)
                .OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Employee_Type>() // Employee to Employee Type
            //     .HasMany(e => e.Employees)
            //     .WithOne(et => et.Employee_Types) // 1 E to M ET
            //     .HasForeignKey(fk => fk.Employee_Type_Id)
            //     .IsRequired()
            //     .OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Room_Type>() // Employee to Employee Type
            //  .HasMany(c => c.Rooms)
            //  .WithOne(rt => rt.Room_Types) // 1 E to M ET
            //  .HasForeignKey(fk => fk.Room_Type_Id)
            //  .IsRequired()
            //  .OnDelete(DeleteBehavior.NoAction);


            // modelBuilder.Entity<Client>() // Employee to Employee Type
            // .HasMany(c => c.Room_Bookings)
            // .WithOne(c => c.Clients) // 1 E to M ET
            // .HasForeignKey(fk => fk.Client_Id)
            // .IsRequired()
            // .OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Room>() // Employee to Employee Type
            //.HasMany(c => c.Room_Bookings)
            //.WithOne(r => r.Rooms) // 1 E to M ET
            //.HasForeignKey(fk => fk.Room_Id)
            //.IsRequired()
            //.OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Room_Type>()
            //     .HasMany(rb => rb.Rooms_Booking)
            //     .WithOne(rt => rt.Rooms_Type)
            //     .HasForeignKey(fk => fk.Room_Type_Id)
            //     .IsRequired()
            //     .OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Booking_Review>();

        }





        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                    new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                    new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
                    new IdentityRole() { Name = "Manager", ConcurrencyStamp = "3", NormalizedName = "Manager" },
                    new IdentityRole() { Name = "Employee", ConcurrencyStamp = "4", NormalizedName = "Employee" },
                    new IdentityRole() { Name = "None", ConcurrencyStamp = "5", NormalizedName = "None" }
                );
        }




    }

}
