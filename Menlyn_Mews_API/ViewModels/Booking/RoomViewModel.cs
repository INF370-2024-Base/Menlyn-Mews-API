namespace Menlyn_Mews_API.ViewModels.Booking
{
    public class RoomViewModel
    {
        public int? Room_Number { get; set; }
        public int? Room_Floor { get; set; }
        public string? Room_Status { get; set; }
        public Decimal Room_Rate { get; set; }
        public string? Room_Description { get; set; }
        public int RoomTypeId { get; set; }
    }
}
