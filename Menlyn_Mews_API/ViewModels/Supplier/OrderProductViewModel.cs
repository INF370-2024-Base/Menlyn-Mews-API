namespace Menlyn_Mews_API.ViewModels.Supplier
{
    public class OrderProductViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public int? ReceiveOrderId { get; set; }
    }
}
