using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public string Inventory_Name { get; set; }
        public int Maximum_Stock { get; set; }
        public int Quantity_Available { get; set; }
        public decimal Price_Per_Unit { get; set; }

        //------------------------------------FK-----------------------------------//
        public int InventoryTypeId { get; set; }
        public int InventoryCategoryId { get; set; }

        public Inventory_Type InventoryType {  get; set; }  
        public Inventory_Category InventoryCategory { get; set; }

        //Related Table

        public virtual ICollection<Stock_Take> Stock_Take { get; set;}

        public virtual ICollection<Product> Products { get; set; }  

        public virtual ICollection<Room_Inventory> Room_Inventory { get; set; }

    }
}

