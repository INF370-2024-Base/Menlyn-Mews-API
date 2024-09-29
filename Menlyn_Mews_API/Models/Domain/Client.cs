using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Client
    {

        [Key]
        public int ClientId { get; set; }

        public string? Client_Name { get; set; } = string.Empty;

        public string? Client_Surname { get; set; } = string.Empty;

        public string? Client_ID_Number { get; set; }

        public string? Client_Email_Address { get; set; } = string.Empty;

        public string? Client_Contact_Number { get; set; }

        public string? Client_Gender { get; set; } = string.Empty;

        public string? Title { get; set; } = string.Empty;

        ///------------------------FK--------------------///

        public string? ApplicationUserId {  get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }


        [JsonIgnore]
        public virtual ICollection<Room_Booking>? Room_Bookings { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking_Review>? Booking_Reviews { get; set; }

        [JsonIgnore]
        public virtual ICollection<Event_Review>? Event_Reviews { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Referrals>? Referrals { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Event_Booking>? Event_Booking { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Payment>? Payments { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Complaint>? Complaint { get; set; }

        [JsonIgnore]
        public virtual ICollection<Client_Discount>? Client_Discounts { get; set; }
    }
}
