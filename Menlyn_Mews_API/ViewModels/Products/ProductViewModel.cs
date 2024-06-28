namespace Menlyn_Mews_API.ViewModels.Products
{
    public class ProductViewModel
    {
        public string Product_Name { get; set; }
        public int Quantity_On_Hand { get; set; }
        public int Product_Type_Id { get; set; }
        public int Product_Category_Id { get; set;}
        public int Price_Id {  get; set;}
        public int Inventory_Id { get; set;}

    }
}
