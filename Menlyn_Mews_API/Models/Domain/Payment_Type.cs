
using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Payment_Type
    {
        [Key]
        public int PaymentTypeId { get; set; }
        public string Payment_Type_description { get; set; }

        //Related Tables

    }
}
