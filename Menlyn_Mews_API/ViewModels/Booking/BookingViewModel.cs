using Menlyn_Mews_API.Models.Domain;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.ViewModels.Booking
{
    public class BookingViewModel
    {
        public string? Check_In_Date { get; set; }

        public string? Check_Out_Date { get; set; }

        public int? Booking_Price { get; set; }

        public int ClientId { get; set; }

        public int Room_Id { get; set; }

        public int Room_Type_Id { get; set; }
    }
}
