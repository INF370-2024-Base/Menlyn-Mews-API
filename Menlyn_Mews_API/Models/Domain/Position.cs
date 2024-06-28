using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Position
    {
        [Key] 
        public int PositionId { get; set; }

        public string? Position_Description { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<Employee>? Employee { get; set; }
    }
}
