using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Price
    {
        [Key]
        public int PriceId { get; set; }
        public Decimal Product_Price {  get; set; } = int.MinValue;
        public DateTime Price_Date { get; set; } = DateTime.MinValue;

        public virtual ICollection<Product> Product { get; set; }

    }
}
