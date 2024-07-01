namespace Menlyn_Mews_API.ViewModels.Inventory
{
    public class StockTakeViewModel
    {
        public DateTime Stock_Take_Date { get; set; }
        public int Total_Items { get; set; }
        public Decimal Total_Value { get; set; }

        //FK
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public int InventoryId { get; set; }

    }
}
