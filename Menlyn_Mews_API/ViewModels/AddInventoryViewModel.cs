namespace Menlyn_Mews_API.ViewModels
{
    public class AddInventoryViewModel
    {
        public string Inventory_Name { get; set; }
        public int Minimum_Stock { get; set; }
        public int Maximum_Stock { get; set; }
        public string Inventory_Condition { get; set; }
        public string Inventory_Status { get; set; }

        //Related Data
        public int InventoryTypeId { get; set; }
        public int InventoryCategoryId { get; set; }
    }
}
