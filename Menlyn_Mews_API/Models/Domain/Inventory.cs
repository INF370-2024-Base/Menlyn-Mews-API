

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Inventory Name is required")]
        public string? Inventory_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Minimum Stock Is Required")]
        public int? Minimum_Stock { get; set; } = int.MinValue;

        [Required(ErrorMessage = "Maximum Stock Is Required")]
        public int? Maximum_Stock { get; set; } = int.MinValue;

        [Required(ErrorMessage = "Inventory Condition is required")]
        public string? Inventory_Condition { get; set; } = "Good";

        [Required(ErrorMessage = "Inventory Status Required")]
        public string? Inventory_Status { get; set; } = "Empty";

        //------------------------------------FK-----------------------------------//
        public int Inventory_Type_Id { get; set; }
        [JsonIgnore]
        public Inventory_Type? Inventory_Types { get; set; } // Navigation property

        public int Inventory_Category_Id { get; set; }
        [JsonIgnore]
        public Inventory_Category? Inventory_Categories { get; set; } // Navigation property

    }
}

