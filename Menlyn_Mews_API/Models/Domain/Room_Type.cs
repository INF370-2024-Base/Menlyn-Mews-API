using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Room_Type
    {
        [Key]
        public int RoomTypeId { get; set; }

        public string? Room_Type_Description { get; set; } = string.Empty;

        public int? Room_Type_Capacity { get; set; } = 1;

        public string? Room_Size { get; set; } = string.Empty;


        //--------------------------------------------FK-----------------------------------------//
        [JsonIgnore]
        public virtual ICollection<Room> Rooms { get; set; }

    }
}

