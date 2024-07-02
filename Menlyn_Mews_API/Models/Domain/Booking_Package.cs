using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Booking_Package
    {
        [Key]
        public int BookingPackageId { get; set; }
        public string Booking_Package_Name { get; set; }
        public string Booking_Package_Description { get; set; }
        public Decimal Booking_Package_Price { get; set; }

        //Related Tables
        [JsonIgnore]
        public virtual ICollection<Room_Booking>? Room_Booking { get; set; }
    }
}
