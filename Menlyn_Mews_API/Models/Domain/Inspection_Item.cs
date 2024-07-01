using System.ComponentModel.DataAnnotations;
using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Inspection_Item
    {
        [Key]
        public int InspectionItemId { get; set; }
        public string Inspection_Item_Name { get; set; }    
        public string Inspection_Item_Condition { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

    }
}
