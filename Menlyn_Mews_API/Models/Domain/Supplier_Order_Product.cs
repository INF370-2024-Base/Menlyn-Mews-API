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

        public string Order_Product_Description { get; set; } = string.Empty;

        public int ReceiveOrderId { get; set; }
        public Receive_Order Receive_Order { get; set; }

    }
}
