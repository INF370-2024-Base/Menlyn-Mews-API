using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Room
    {

        [Key]
        public int RoomId { get; set; }

        public int? Room_Number { get; set; }

        public int? Room_Floor { get; set; }

        public string? Room_Status { get; set; } = string.Empty;

        public Decimal Room_Rate { get; set; }

        public string? Room_Description { get; set; } = string.Empty;

        public string? Room_Photo_1 { get; set; } 
        
        public string? Room_Photo_2 { get; set; }

        public string? Room_Photo_3 { get; set; }

        public string? Room_Photo_4 { get; set; }

        public string? Room_Photo_5 { get; set; }

        public string? Room_Photo_6 { get; set; }

        public string? Room_Photo_7 { get; set; }

        public string? Room_Photo_8 { get; set; }

        public string? Room_Photo_9 { get; set; }

        public string? Room_Photo_10 { get; set; }

        public string? Amenities { get; set; }

        //------------------------------------FK-----------------------------------//

        public int RoomTypeId { get; set; }
        [JsonIgnore]
        public Room_Type Room_Type { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking_Review> Booking_Review {  get; set; }    

        [JsonIgnore] 
        public virtual ICollection<Room_Booking> Room_Bookings { get; set; }

        [JsonIgnore]
        public virtual ICollection<Inventory> Inventories { get; set; }

        [JsonIgnore] 
        public virtual ICollection<Room_Inventory> Room_Inventory { get; set; }


    }
}

