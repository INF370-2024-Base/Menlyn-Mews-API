

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public string? Inventory_Name { get; set; } = string.Empty;
        public int? Minimum_Stock { get; set; } = int.MinValue;
        public int? Maximum_Stock { get; set; } = int.MinValue;
        public string? Inventory_Condition { get; set; } = string.Empty;
        public string? Inventory_Status { get; set; } = string.Empty;

        //------------------------------------FK-----------------------------------//
        
        public int InventoryTypeId { get; set; }
        public int InventoryCategoryId { get; set; }

        public Inventory_Type InventoryType {  get; set; }  
        public Inventory_Category InventoryCategory { get; set; }   

    }
}

