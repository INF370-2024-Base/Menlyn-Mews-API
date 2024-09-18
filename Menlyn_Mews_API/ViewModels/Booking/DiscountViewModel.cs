namespace Menlyn_Mews_API.ViewModels.Booking
{
    public class DiscountViewModel
    {
        public string Discount_Name { get; set; } 
        public string Discount_Code { get; set; }
        public Decimal Discount_Percenatage { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public bool Is_Active { get; set; }
        public bool email_Sent { get; set; }
    }
}
