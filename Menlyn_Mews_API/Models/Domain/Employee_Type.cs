using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Employee_Type
    {
        [Key] // Specify the primary key
        public int Id { get; set; }

        public string? Type_Description { get; set; } = string.Empty;


        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}
