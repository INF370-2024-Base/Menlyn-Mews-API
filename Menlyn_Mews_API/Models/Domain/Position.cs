using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Position
    {
        [Key] 
        public int PositionId { get; set; }

        public string? Position_Description { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
