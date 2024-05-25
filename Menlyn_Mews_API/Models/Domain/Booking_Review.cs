using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Booking_Review
    {

        [Key]
        public int Id { get; set; }

        public string? Review_Status { get; set; } = "Completed";

        public int? Review_Rating { get; set; } = 1;

        public string? Review_Description { get; set; } = string.Empty;

        public string? Review_ImageUrl {  get; set; }

    }
}
