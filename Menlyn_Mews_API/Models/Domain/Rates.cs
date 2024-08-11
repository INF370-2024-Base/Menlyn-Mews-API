using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Rates
    {
        [Key]
        public int RateId { get; set; }
        public string Employee_Type { get; set; }
        public decimal Rate { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
