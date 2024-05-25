using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Client
    {

        [Key]
        public int Id { get; set; }

        public string? Client_Name { get; set; } = string.Empty;

        public string? Client_Surname { get; set; } = string.Empty;

        //menu dropdown
        public int? Client_ID_Number { get; set; }

        public string? Client_Email_Address { get; set; } = string.Empty;

        //menu dropdown
        public int? Client_Contact_Number { get; set; }

        public string? Client_Gender { get; set; } = string.Empty;

        //Might be deleted since it is a dropdown as Client Gender is really important

        public string? Client_Address { get; set; } = string.Empty;

        public string? Title { get; set; } = string.Empty;

        ///------------------------FK--------------------///


        [JsonIgnore] // might delete later, it was not here before, just testing out the JSON ignore on APIs
        public List<Room_Booking>? Room_Bookings { get; set; }
    }
}
