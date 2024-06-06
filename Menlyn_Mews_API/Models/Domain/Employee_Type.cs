using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Employee_Type
    {
        [Key] // Specify the primary key
        public int EmployeeTypeId { get; set; }

        public string? Type_Description { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
