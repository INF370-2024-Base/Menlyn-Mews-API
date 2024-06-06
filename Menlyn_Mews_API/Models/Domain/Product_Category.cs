using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Product_Category
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public string Product_Category_Name { get; set; } = string.Empty;   
        public string Product_Category_Description { get; set; } = string.Empty;

        //Related Tables 
        public virtual ICollection<Product> Product { get; set; }
    }
}
