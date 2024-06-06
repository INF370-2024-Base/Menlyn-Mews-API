using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Supplier_Type
    {
        [Key]
        public int SupplierTypeId { get; set; }
        public string Supplier_Type_Description { get; set; } = string.Empty;

        public virtual ICollection<Supplier> Suppliers { get; set; }

    }
}
