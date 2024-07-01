using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }
        public string Discount_Name { get; set; } = string.Empty;
        public string Discount_Code {  get; set; } = string.Empty;  
        public Decimal Discount_Percenatage { get; set; }
        public DateTime Start_Date { get; set; }    
        public DateTime End_Date { get; set; }
        public bool Is_Active { get; set; }

        public virtual ICollection<Room_Booking>? Room_Booking { get; set;}
    }
}
