namespace Library.Shoop.Models // Note: actual namespace depends on the project name.
{
    public partial class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Id { get; set; }
        public double TotalPrice
        {
            get
            {
                return Price * Quantity;
            }
        }

        public Product()
        {
            Name = "";
            Description = "";
        }

        public override string ToString()
        {
            return $"{Id} - {Name} - {Description} - {Price} - {Quantity} - ${TotalPrice}";
        }

    }
}