namespace Menlyn_Mews_API.ViewModels.Booking
{
    public class PaymentViewModel
    {
        public Decimal Payment_Amount { get; set; }
        public int PaymentTypeId { get; set; }
        public int ClientId { get; set; }
    }
}
