using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Menlyn_Mews_API.Models.Domain
{
    public class Room_Booking
    {
        [Key]
        public int RoomBookingId { get; set; }

        public string? Check_In_Date { get; set; }

        public string? Check_Out_Date { get; set; }

        public string? Booking_Status { get; set; } = string.Empty;

        public int? Booking_Price { get; set; }


        ///------------------------FK--------------------///
        //Change These
        public int ClientId { get; set; }
        public Client? Clients { get; set; }


        public int Room_Id { get; set; }
        [JsonIgnore]
        public Room? Rooms { get; set; }

        public int Room_Type_Id { get; set; }
        [JsonIgnore]
        public Room_Type? Rooms_Type { get; set; }

    }
}
