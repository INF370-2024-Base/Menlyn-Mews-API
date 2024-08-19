using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Inspection_Item
    {
        [Key]
        public int InspectionItemId { get; set; }
        public DateTime Inspection_Date { get; set; }    
        public string Inspection_Status { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }

        [JsonIgnore]
        public Employee_Shift Employee_Shift { get; set; }

        [JsonIgnore]
        public virtual ICollection<Write_Off> Write_Off { get; set; }

    }
}
