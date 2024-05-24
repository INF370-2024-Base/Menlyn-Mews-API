using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Employee
    {
        [Key] // Specify the primary key
        public int Employee_Id { get; set; }

        public string? Employee_Name { get; set; } = string.Empty;

        public string? Employee_Surname { get; set; } = string.Empty;

        public int? Employee_ID_Number { get; set; } = 0;

        public string? Employee_Email_Address { get; set; } = string.Empty;

        public int? Employee_Contact_Number { get; set; } = 0;

        public string? Employee_Gender { get; set; } = string.Empty;
        public string? Employee_Address { get; set; } = string.Empty;


        //------------------------------------FK-----------------------------------//
        public int Employee_Type_Id { get; set; }
        [JsonIgnore]
        public Employee_Type? Employee_Types { get; set; } // Navigation property

    }
}
