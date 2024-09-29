namespace Menlyn_Mews_API.ViewModels.Inventory
{
    public class WriteOffViewModel
    {
        public string Write_Off_Description { get; set; }
        public int Quantity_Of_Items_Written_Off { get; set; }

        //----FK---//
        public int RoomId { get; set; }
        public int InventoryId { get; set; }
        public int InspectionItemId { get; set; }
        public int EmployeeId { get; set; }
        public int RoomBookingId { get; set; }
    }
}
