using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Product_Category
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public string Product_Category_Name { get; set; }    
        public string Product_Category_Description { get; set; }

        public virtual ICollection<Product_Type>? Product_Type { get; set; } 

        //Related Tables 
        [JsonIgnore]
        public virtual ICollection<Product> Product { get; set; }
    }
}
