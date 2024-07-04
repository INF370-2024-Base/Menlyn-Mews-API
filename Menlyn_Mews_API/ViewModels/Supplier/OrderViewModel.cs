namespace Menlyn_Mews_API.ViewModels.Supplier
{
    public class OrderViewModel
    {
        public string Order_Description { get; set; }
        public DateTime Order_Date { get; set; } 
        public string Order_Status { get; set; }
        public int SupplierId { get; set; }
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
    }
}
