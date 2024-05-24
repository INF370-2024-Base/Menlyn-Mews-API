

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Inventory_Type
    {
        [Key]
        public int Id { get; set; }

        public string? Inventory_Type_Name { get; set; } = string.Empty;

        public string? Inventory_Type_Description { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Inventory>? Inventories { get; set; }
    }
}

