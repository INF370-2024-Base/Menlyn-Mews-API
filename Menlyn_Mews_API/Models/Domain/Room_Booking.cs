using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Menlyn_Mews_API.Models.Domain
{
    public class Room_Booking
    {
        [Key]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? Check_In_DateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? Check_Out_DateTime { get; set; }

        public string? Booking_Status { get; set; } = "Booked";

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number")]
        public int? Booking_Price { get; set; }


        ///------------------------FK--------------------///

        public int Client_Id { get; set; }
        [JsonIgnore]
        public Client? Clients { get; set; }


        public int Room_Id { get; set; }
        [JsonIgnore]
        public Room? Rooms { get; set; }


    }
}
