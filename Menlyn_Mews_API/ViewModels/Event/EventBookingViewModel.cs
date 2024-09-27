
namespace Menlyn_Mews_API.ViewModels.Event
{
    public class EventBookingViewModel
    {
        public DateTime Event_Date { get; set; }
        public Decimal Event_Price { get; set; }
        public DateTime? Start_Time { get; set; }
        public DateTime? End_Time { get; set; }
        public string Event_Status { get; set; }
        public string Allergy_Description { get; set; }
        public int EventTypeId { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date_Sent { get; set; }
    }
}
