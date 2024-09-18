using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public int Quantity_On_Hand { get; set; } = int.MinValue;

        //Related Tables
        public int PriceId { get; set; }
        public Price Price { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public int ProductTypeId { get; set; }
        public Product_Type ProductType { get; set; }
 
        //Bridge
        public virtual ICollection<Supplier_Order_Product> Supplier_Order_Product { get; set; }
    }
}
