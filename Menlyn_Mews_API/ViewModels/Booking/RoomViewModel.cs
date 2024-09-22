namespace Menlyn_Mews_API.ViewModels.Booking
{
    public class RoomViewModel
    {
        public int? Room_Number { get; set; }

        public int? Room_Floor { get; set; }

        public string? Room_Status { get; set; }

        public Decimal Room_Rate { get; set; }

        public string? Room_Description { get; set; }

        public IFormFile? Room_Photo_1 { get; set; }

        public IFormFile? Room_Photo_2 { get; set; }

        public IFormFile? Room_Photo_3 { get; set; }

        public string? Amenities { get; set; }
        public int RoomTypeId { get; set; }
    }
}
