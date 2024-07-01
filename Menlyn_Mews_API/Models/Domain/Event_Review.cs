using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Event_Review
    {
        [Key]
        public int EventReviewId { get; set; }

        public string? Event_Review_Status { get; set; } = string.Empty;

        public int? Event_Review_Rating { get; set; } = 1;

        public string? Event_Review_Description { get; set; } = string.Empty;

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
