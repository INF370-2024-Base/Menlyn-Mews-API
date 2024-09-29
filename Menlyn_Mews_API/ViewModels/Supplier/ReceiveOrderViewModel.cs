namespace Menlyn_Mews_API.ViewModels.Supplier
{
    public class ReceiveOrderViewModel
    {
        public string Received_By { get; set; }
        public string Received_Status { get; set; } 
        public List<ProductReceivedViewModel> ProductsReceived { get; set; } = new List<ProductReceivedViewModel>();
    }
}
