using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Complaint
    {
        [Key]
        public int ComplaintId { get; set; }
        public string Complaint_Description { get; set; }
        public DateTime Complaint_Date { get; set; }
        public string Complaint_Status { get; set; }

        //FK
        public int? EmployeeId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }

        public int ClientId { get; set; }
        [JsonIgnore]
        public Client Client { get; set; }

        public int ComplaintTypeId { get; set; }
        [JsonIgnore]
        public Complaint_Type Complaint_Type { get; set; }
    }
}
