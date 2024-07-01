using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public DateTime Payment_Date { get; set; }
        public Decimal Payment_Amount { get; set; }
        public string Payment_Status { get; set; }

        //FK
        public int PaymentTypeId { get; set; }
        public Payment_Type Payment_Type { get; set; }

        public int ClientId { get; set; }   
        public Client Client { get; set; }
    }
}
