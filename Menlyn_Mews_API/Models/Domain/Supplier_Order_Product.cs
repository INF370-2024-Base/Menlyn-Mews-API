using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Supplier_Order_Product
    {
        [Key]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }  
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public int? ReceiveOrderId { get; set; }
        public Receive_Order? Receive_Order { get; set; }

    }
}
