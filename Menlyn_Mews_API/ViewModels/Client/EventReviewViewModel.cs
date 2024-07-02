namespace Menlyn_Mews_API.ViewModels.Client
{
    public class EventReviewViewModel
    {
        public string? Event_Review_Status { get; set; }
        public int? Event_Review_Rating { get; set; } 
        public string? Event_Review_Description { get; set; }
        public int ClientId { get; set; }
    }
}
