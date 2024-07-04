using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Event_Booking
    {
        [Key] 
        public int EventId { get; set; }
        public DateTime Event_Date { get; set; }
        public Decimal Event_Price { get; set; }
        public TimeSpan? Start_Time { get; set; }
        public TimeSpan? End_Time { get; set; }
        public string Event_Status { get; set; } = string.Empty;    
        public string Allergy_Description {  get; set; } = string.Empty;

        //FK
        public int EventTypeId { get; set; }
        [JsonIgnore]
        public Event_Types Event_Types { get; set; }

        public int ClientId { get; set; }
        [JsonIgnore]
        public Client Client { get; set; }

        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        [JsonIgnore]    
        public Employee_Shift? Employee_Shift { get; set; }

    }
}
