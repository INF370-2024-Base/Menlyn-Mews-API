using Menlyn_Mews_API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menlyn_Mews_API.ViewModels.Employee
{
    public class EmployeeShiftViewModel
    {
        public DateTime Shift_Date { get; set; }
        public DateTime Clock_In_Time { get; set; }
        public DateTime Clock_Out_Time { get; set; }
        public string Shift_Description { get; set; }
    }
}
