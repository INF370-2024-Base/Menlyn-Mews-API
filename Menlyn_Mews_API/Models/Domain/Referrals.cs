using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Referrals
    {
        [Key]
        public int ReffaralId { get; set; }
        public string Ref_Discount_Code { get; set; }   
        public Decimal Discount_Percenatge { get; set; }
        public DateTime Refferal_Date { get; set; }
        public DateTime Redemption_Date { get; set; }
        public string Redemption_Status { get; set; }
        public DateTime Expiration_Date { get; set; }

        //FK
        public int ClientId { get; set; }
        public Client? Client { get; set; }  
    }
}
