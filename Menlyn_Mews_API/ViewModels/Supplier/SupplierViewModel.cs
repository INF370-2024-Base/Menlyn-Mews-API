namespace Menlyn_Mews_API.ViewModels.Supplier
{
    public class SupplierViewModel
    {
        public string Supplier_Name { get; set; } 
        public string Supplier_Address_Line_1 { get; set; } 
        public string Supplier_Address_Line_2 { get; set; } 
        public string Supplier_Email { get; set; } 
        public string Supplier_Contact_Number { get; set; } 
        public string Supplier_Rep_Name { get; set; } 
        public string Supplier_Rep_Surname { get; set; } 
        public string Supplier_Rep_Email { get; set; } 
        public string Supplier_Rep_Contact_Number { get; set; } 
        public string City { get; set; } 
        public string Province { get; set; } 
        public string Postal_Code { get; set; } 
        public int SupplierTypeId { get; set; }
    }
}
