namespace Menlyn_Mews_API.Models.Domain
{
    public class Employee_Shift
    {
        //Bridge Keys
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ShiftId { get; set; }
        public Shift Shift { get; set; }

        public TimeOnly Clock_In_Time { get; set; }
        public TimeOnly Clock_Out_Time { get; set; }
        public string Shift_Description { get; set; } = string.Empty;

        //Related Tables

        public virtual ICollection<Order> Order { get; set; }   
        public virtual ICollection<Room_Booking> Room_Booking { get; set; }
    }
}
