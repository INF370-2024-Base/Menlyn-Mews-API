namespace Menlyn_Mews_API.ViewModels.Client
{
    public class BookingReviewViewModel
    {
        public string? Review_Status { get; set; }
        public int? Review_Rating { get; set; }
        public string? Review_Description { get; set; }
        public DateTime Date_Created { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
    }
}
