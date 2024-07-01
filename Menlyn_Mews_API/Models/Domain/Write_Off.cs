using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Write_Off
    {
        [Key]
        public int WriteOffId { get; set; }
        public string Write_Off_Description { get; set; }
        public DateTime Write_Off_Date { get; set;}
        public string Write_Off_Stock_Type_Name { get; set;}
        public string Write_Off_Stock_Type_Description { get; set; }

        //----FK---//
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }    

        public int EmployeeId { get; set; } 
        public int ShiftId { get; set; }    
        public Employee_Shift? Employee_Shift { get; set; }

    }
}
