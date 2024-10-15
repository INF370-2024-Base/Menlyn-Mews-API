using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Event_Types
    {
        [Key]
        public int EventTypeId { get; set; } 
        public string Event_Description { get; set; }   
        public string Event_Capacity_Status { get; set; }

        public string Event_Type_Name { get; set; }
      
        public Decimal Event_Type_Price { get; set; }

        public int Event_Capacity { get; set; }

        //Related Tables
        [JsonIgnore]
        public virtual ICollection<Event_Booking> Event_Booking { get; set; }  
    }
}
