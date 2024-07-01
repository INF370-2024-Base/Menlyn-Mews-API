using System.ComponentModel.DataAnnotations;

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
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int ComplaintTypeId { get; set; }
        public Complaint_Type Complaint_Type { get; set; }
    }
}
