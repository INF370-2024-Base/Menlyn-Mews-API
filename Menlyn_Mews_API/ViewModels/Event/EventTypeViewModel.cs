namespace Menlyn_Mews_API.ViewModels.Event
{
    public class EventTypeViewModel
    {
        public string Event_Description { get; set; }
        public string Event_Capacity_Status { get; set; }
        public string Event_Type_Name { get; set; }

        public Decimal Event_Type_Price { get; set; }

        public int Event_Capacity { get; set; }
    }
}
