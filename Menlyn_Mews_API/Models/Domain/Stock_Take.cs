using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Stock_Take
    {
        [Key]
        public int StockTakeId { get; set; }
        public DateTime Stock_Take_Date { get; set; }
        public int Total_Items { get; set; }
        public Decimal Total_Value { get; set; }

        //------FK-----//
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public Employee_Shift Employee_Shift { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        //-------Related Tables--------//

    }
}
