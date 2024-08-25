using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Audit_Log
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Action { get; set; }
        public string Controller_Name { get; set; }
        public string Action_Name { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}
