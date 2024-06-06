namespace Menlyn_Mews_API.Models.Domain
{
    public class Supplier_Order_Product
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }  
        public Product Product { get; set; }

        public string Order_Product_Description { get; set; } = string.Empty;

        //Bridge
        public virtual ICollection<Receive_Supplier_Order> Receive_Supplier_Order { get; set; }

    }
}
