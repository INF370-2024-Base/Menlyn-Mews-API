namespace Menlyn_Mews_API.Models.Domain
{
    public class Prod_Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Prod_Category_Id { get; set; }
    }
}
