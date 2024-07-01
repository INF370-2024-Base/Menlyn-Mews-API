using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Audit_Log
    {
        [Key]
        public int LogId { get; set; }
        public string Log_Description { get; set; }
        public DateTime Log_Date_Time { get; set; }

        //FK
        public int EmployeeId { get; set; } 
        public Employee Employee { get; set; }  
    }
}
