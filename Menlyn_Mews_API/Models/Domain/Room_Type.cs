using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Room_Type
    {
        [Key]
        public int RoomType_Id { get; set; }

        public string? RoomType_Description { get; set; } = string.Empty;

        public int? RoomType_Capacity { get; set; } = 1;

        public string? Room_Size { get; set; } = string.Empty;


        //--------------------------------------------FK-----------------------------------------//
        [JsonIgnore]
        public List<Room>? Rooms { get; set; }
    }
}

