using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Event_Types
    {
        [Key]
        public int EventTypeId { get; set; } 
        public string Event_Description { get; set; }   
        public string Event_Capacity_Status { get; set; }

        //Related Tables
        public virtual ICollection<Event_Booking> Event_Booking { get; set; }  
    }
}
