using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Write_Off
    {
        [Key]
        public int WriteOffId { get; set; }
        public string Write_Off_Description { get; set; }
        public int Quantity_Of_Items_Written_Off { get; set; }

        //----FK---//
        public int RoomId { get; set; }
        public int InventoryId { get; set; }
        public Room_Inventory Room_Inventory { get; set; }  

        public int InspectionItemId { get; set; }
        [JsonIgnore]
        public Inspection_Item Inspection_Item { get; set; }

        public int EmployeeId { get; set; } 
        public int ShiftId { get; set; }    
        public Employee_Shift? Employee_Shift { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

    }
}
