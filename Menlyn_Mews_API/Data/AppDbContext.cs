using Microsoft.EntityFrameworkCore; // added 13 / 04 / 2024
using Menlyn_Mews_API.Models.Domain; // added 13 / 04 / 2024

namespace Menlyn_Mews_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee_Type>() // Employee to Employee Type
                .HasMany(e => e.Employees)
                .WithOne(et => et.Employee_Types) // 1 E to M ET
                .HasForeignKey(fk => fk.Employee_Type_Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Client>() // Employee to Employee Type
               .HasMany(c => c.Booking_Reviews)
               .WithOne(br => br.Clients) // 1 E to M ET
               .HasForeignKey(fk => fk.Client_Id)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);




            modelBuilder.Entity<Inventory_Type>() // Employee to Employee Type
               .HasMany(c => c.Inventories)
               .WithOne(it => it.Inventory_Types) // 1 E to M ET
               .HasForeignKey(fk => fk.Inventory_Type_Id)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Inventory_Category>() // Employee to Employee Type
              .HasMany(c => c.Inventories)
              .WithOne(ic => ic.Inventory_Categories) // 1 E to M ET
              .HasForeignKey(fk => fk.Inventory_Category_Id)
              .IsRequired()
              .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Room_Type>() // Employee to Employee Type
             .HasMany(c => c.Rooms)
             .WithOne(rt => rt.Room_Types) // 1 E to M ET
             .HasForeignKey(fk => fk.Room_Type_Id)
             .IsRequired()
             .OnDelete(DeleteBehavior.NoAction);


            //modelBuilder.Entity<Client>() // Employee to Employee Type
            //.HasMany(c => c.Room_Bookings)
            //.WithOne(c => c.Clients) // 1 E to M ET
            //.HasForeignKey(fk => fk.Client_Id)
            //.IsRequired()
            //.OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room>() // Employee to Employee Type
           .HasMany(c => c.Room_Bookings)
           .WithOne(r => r.Rooms) // 1 E to M ET
           .HasForeignKey(fk => fk.Room_Id)
           .IsRequired()
           .OnDelete(DeleteBehavior.NoAction);






        }

    }
}
