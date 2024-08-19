using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Room_Inventory
    {
        //Bridge Keys
        [Key]
        [ForeignKey("Room")]
        public int RoomId { get; set; } 
        public Room Room { get; set; }

        [Key]
        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }    


        //Related Tables
        public virtual ICollection<Write_Off> Write_Off { get; set; }

    }
}
