using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Complaint_Type
    {
        [Key]
        public int ComplaintTypeId { get; set; }
        public string Complaint_Type_Description { get; set; }

        //Related Tables
        public virtual ICollection<Complaint> Complaint { get; set; }
    }
}
