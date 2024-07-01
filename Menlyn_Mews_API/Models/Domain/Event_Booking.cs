using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Event_Booking
    {
        [Key] 
        public int EventId { get; set; }
        public DateTime Event_Date { get; set; }
        public Decimal Event_Price { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public string Event_Status { get; set; } = string.Empty;    
        public string Allergy_Description {  get; set; } = string.Empty;

        //FK
        public int EventTypeId { get; set; }
        public Event_Types Event_Types { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public Employee_Shift? Employee_Shift { get; set; }

    }
}
