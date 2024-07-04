using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string Order_Description { get; set; } = string.Empty;
        public DateTime Order_Date { get; set; } = DateTime.MinValue;
        public string Order_Status {  get; set; } = string.Empty;


        //Related Tables
        public int SupplierId { get; set; }
        [JsonIgnore]
        public virtual Supplier Suppliers { get; set; }

        //Employee_Shift Bridge
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        [JsonIgnore]
        public Employee_Shift Employee_Shift { get; set; }

        //Bridge
        [JsonIgnore]
        public virtual ICollection<Supplier_Order_Product> Supplier_Order_Product { get; set; }
    }
}
