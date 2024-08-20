using Menlyn_Mews_API.Models.Domain;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.ViewModels.Booking
{
    public class BookingNoDisViewModel
    {
        public DateTime? Check_In_Date { get; set; }
        public DateTime? Check_Out_Date { get; set; }
        public string? Booking_Status { get; set; }
        public int? Booking_Price { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public int? BookingPackageId { get; set; }

    }
}
