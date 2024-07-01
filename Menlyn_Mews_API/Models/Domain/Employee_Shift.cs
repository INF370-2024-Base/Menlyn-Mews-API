using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Employee_Shift
    {
        //Bridge Keys
        [Key]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Key]
        [ForeignKey("Shift")]
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }

        public DateTime Clock_In_Time { get; set; }
        public DateTime Clock_Out_Time { get; set; }
        public string Shift_Description { get; set; } = string.Empty;

        //FK

        //Related Tables

        public virtual ICollection<Order> Order { get; set; }   

        public virtual ICollection<Write_Off> Write_Off { get; set; }
        public virtual ICollection<Stock_Take> Stock_Take { get; set;}

        public virtual ICollection<Event_Booking> Event_Booking { get; set; }   
    }
}
