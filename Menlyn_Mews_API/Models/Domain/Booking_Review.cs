using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Booking_Review
    {
        // PLEASE RENAME IDs WITH MODEL NAME

        [Key]
        public int BookingReviewId { get; set; }

        public string? Review_Status { get; set; } = string.Empty;

        public int? Review_Rating { get; set; } = 1;

        public string? Review_Description { get; set; } = string.Empty;

        public DateTime Date_Created { get; set; }

        //---------------------------------------------FK---------------------------------------------//

        public int ClientId { get; set; }

        [JsonIgnore]
        public Client Client { get; set; }

        public int RoomId { get; set; }
        [JsonIgnore]
        public Room? Room { get; set; }

        public int RoomBookingId { get; set; }
        [JsonIgnore]
        public Room_Booking Room_Booking { get; set; }

    }
}
