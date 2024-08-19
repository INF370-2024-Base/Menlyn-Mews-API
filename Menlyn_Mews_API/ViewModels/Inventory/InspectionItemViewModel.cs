namespace Menlyn_Mews_API.ViewModels.Inventory
{
    public class InspectionItemViewModel
    {
        public DateTime Inspection_Date { get; set; }
        public string Inspection_Status { get; set; }
        public int RoomId { get; set; }
        public int Employeeid { get; set; }
        public int ShiftId { get; set; }
    }
}
