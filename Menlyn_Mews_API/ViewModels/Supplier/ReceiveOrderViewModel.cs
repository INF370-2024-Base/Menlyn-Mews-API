namespace Menlyn_Mews_API.ViewModels.Supplier
{
    public class ReceiveOrderViewModel
    {
        public DateTime Received_Order_Date { get; set; } = DateTime.MinValue;
        public string Received_By { get; set; }
        public string Received_Status { get; set; } 
    }
}
