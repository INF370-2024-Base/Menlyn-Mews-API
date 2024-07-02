using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        public DateTime Shift_Date { get; set; }
        public DateTime? Start_TIme { get; set; }
        public DateTime? End_TIme { get; set; }
        public string IP_Address { get; set; } = string.Empty;

        //Related Tables
        //Bridge 
        public virtual ICollection<Employee_Shift> Employee_Shift { get; set; }
    }
}
