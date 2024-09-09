namespace Menlyn_Mews_API.Models.Domain
{
    public class Prod
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity_On_Hand { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public int Prod_Type_Id { get; set; }

        public int Prod_Category_Id { get; set; }

        public int Inventory_Id { get; set; }
    }
}
