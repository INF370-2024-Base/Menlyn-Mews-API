namespace Menlyn_Mews_API.ViewModels.Inventory
{
    public class InventoryViewModel
    {

        public string Inventory_Name { get; set; }
        public int Maximum_Stock { get; set; }
        public int Quantity_Available { get; set; }
        public decimal Price_Per_Unit { get; set; }

        //Related Data
        public int InventoryTypeId { get; set; }
        public int InventoryCategoryId { get; set; }
    }
}
