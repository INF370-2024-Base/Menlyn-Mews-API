using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.ViewModels.Inventory
{
    public class WriteOffViewModel
    {
        public string Write_Off_Description { get; set; }
        public DateTime Write_Off_Date { get; set; }
        public string Write_Off_Stock_Type_Name { get; set; }
        public string Write_Off_Stock_Type_Description { get; set; }

        //----FK---//
        public int InventoryId { get; set; }
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
    }
}
