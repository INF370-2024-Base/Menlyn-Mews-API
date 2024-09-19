namespace Menlyn_Mews_API.ViewModels.Products
{
    public class AddProductViewModel
    {
        public string Product_Name { get; set; }
        public int Quantity_On_Hand { get; set; }
        public int Product_Type_Id { get; set; }
        public int Inventory_Id { get; set; }
        public decimal Product_Price { get; set; }
    }
}
