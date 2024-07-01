using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class VAT
    {
        [Key]
        public int VATId { get; set; }
        public Decimal VAT_Amount { get; set; } 
        public DateTime Creation_Date { get; set; }
        public DateTime Last_Updated { get; set; }
    }
}
