using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Receive_Supplier_Order
    {
        [Key]
        public int OrderId { get; set; }    
        public Order Order { get; set; }    

        public int ProductId { get; set; }  
        public Product Product { get; set; }

        public int ReceieveOrderId { get; set; }
        public Receive_Order Receive_Order { get; set; }

        //Payload
        public string? Address_Line {  get; set; } = string.Empty;
    }
}
