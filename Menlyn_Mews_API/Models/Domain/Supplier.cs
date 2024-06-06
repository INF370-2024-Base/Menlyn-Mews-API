using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        public string Supplier_Name { get; set; } = string.Empty;
        public string Supplier_Address_Line_1 { get; set; } = string.Empty;
        public string Supplier_Address_Line_2 { get; set; } = string.Empty;
        public string Supplier_Email { get; set; } = string.Empty;
        public string Supplier_Contact_Number { get; set; } = string.Empty;
        public string Supplier_Rep_Name { get; set; } = string.Empty;
        public string Supplier_Rep_Surname { get; set; } = string.Empty;
        public string Supplier_Rep_Email { get; set; } = string.Empty;
        public string Supplier_Rep_Contact_Number { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Postal_Code { get; set; } = string.Empty;

        //Related Tables

        public int SupplierTypeId { get; set; }

        public Supplier_Type Supplier_Type { get; set; }
    }
}
