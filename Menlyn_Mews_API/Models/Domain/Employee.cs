using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Employee
    {
        [Key] // Specify the primary key
        public int EmployeeId { get; set; }
        public string? Employee_Name { get; set; } = string.Empty;
        public string? Employee_Surname { get; set; } = string.Empty;
        public string? Employee_ID_Number { get; set; } = string.Empty;
        public string? Employee_Email_Address { get; set; } = string.Empty;
        public string? Employee_Contact_Number { get; set; } = string.Empty;
        public string? Employee_Gender { get; set; } = string.Empty;
        public string? Employee_Address { get; set; } = string.Empty;


        //------------------------------------FK-----------------------------------//
        public int EmployeeTypeId { get; set; }
        public Employee_Type Employee_Type { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }

        public virtual ICollection<Employee_Shift> Employee_Shift { get; set; }

    }
}
