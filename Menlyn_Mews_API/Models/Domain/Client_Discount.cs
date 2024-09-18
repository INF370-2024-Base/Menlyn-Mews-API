using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Client_Discount
    {
        [Key]
        [ForeignKey("Discount")]
        public int DiscountId { get; set; }
        public Discount? Discount { get; set; }

        [Key]
        [ForeignKey("Client")]
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
