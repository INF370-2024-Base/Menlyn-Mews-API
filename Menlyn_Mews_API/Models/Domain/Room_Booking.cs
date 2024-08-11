using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Menlyn_Mews_API.Models.Domain
{
    public class Room_Booking
    {
        [Key]
        public int RoomBookingId { get; set; }

        public DateTime Check_In_Date { get; set; }

        public DateTime Check_Out_Date { get; set; }

        public string? Booking_Status { get; set; } 

        public Decimal? Booking_Price { get; set; }


        ///------------------------FK--------------------///
        public int ClientId { get; set; }
        [JsonIgnore]
        public Client? Clients { get; set; }

        public int RoomId { get; set; }
        [JsonIgnore]
        public Room? Rooms { get; set; }

        public int? BookingPackageId { get; set; }
        [JsonIgnore]
        public Booking_Package? Booking_Package { get; set; }

        public int? DiscountId { get; set; } 
        [JsonIgnore]
        public Discount? Discount {  get; set; } 

    }
}
