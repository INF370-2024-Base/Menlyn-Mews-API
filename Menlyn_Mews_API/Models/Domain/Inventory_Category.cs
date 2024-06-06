


using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Inventory_Category
    {
        [Key]
        public int InventoryCategoryId { get; set; }

        public string? Inventory_Category_Name { get; set; } = string.Empty;

        public string? Inventory_Category_Description { get; set; } = string.Empty;

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}




