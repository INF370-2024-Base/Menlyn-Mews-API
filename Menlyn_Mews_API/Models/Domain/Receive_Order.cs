using Humanizer.Localisation.TimeToClockNotation;
using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Receive_Order
    {
        [Key]
        public int ReceieveOrderId { get; set; }
        public DateTime Received_Order_Date { get; set; } = DateTime.MinValue;
        public string Received_By { get; set; } = string.Empty;
        public string Received_Status {  get; set; } = string.Empty;

        //Related Tables
        public virtual ICollection<Receive_Supplier_Order> Receive_Supplier_Order { get; set; }

    }
}
