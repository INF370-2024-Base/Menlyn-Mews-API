using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Employee
    {
        //DONT CALL PRIMARY KEYS "ID" ONLY ADD A NAME
        [Key] // Specify the primary key
        public int EmployeeId { get; set; }
        public string? Employee_Name { get; set; } = string.Empty;
        public string? Employee_Surname { get; set; } = string.Empty;
        public string? Employee_ID_Number { get; set; } = string.Empty;
        public string? Employee_Email_Address { get; set; } = string.Empty;
        public string? Employee_Contact_Number { get; set; } = string.Empty;
        public string? Employee_Gender { get; set; } = string.Empty;
        public string? Employee_Address { get; set; } = string.Empty;
        public string? Employee_Photo {  get; set; } = string.Empty;


        //------------------------------------FK-----------------------------------//
        public int EmployeeTypeId { get; set; }
        public Employee_Type Employee_Type { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }

        public int RateId { get; set; }
        public Rates Rates {  get; set; }

        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        //Related Tables
        [JsonIgnore]
        public virtual ICollection<Employee_Shift> Employee_Shift { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Audit_Log> Audit_Log { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Complaint> Complaint { get; set; }

        [JsonIgnore]
        public virtual ICollection<Event_Booking> Event_Booking { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Inspection_Item> Inspection_Item { get; set; }
         
        [JsonIgnore]
        public virtual ICollection<Write_Off> Write_Off { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order> Order { get; set; }

    }
}
