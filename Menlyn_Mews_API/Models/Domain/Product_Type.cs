using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Product_Type
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string Product_Type_Name { get; set; } = string.Empty;
        public string Product_Type_Description { get; set; } = string.Empty;

        //Related Taables
        public virtual ICollection<Product> Product { get; set; }
    }
}
