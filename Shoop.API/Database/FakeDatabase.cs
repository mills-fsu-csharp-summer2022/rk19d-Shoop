using Library.Shoop.Models;

namespace Shoop.API.Database
{
    public static class FakeDatabase
    {
        public static List<Product> Products
        {
            get
            {
                var returnList = new List<Product>();
                QuantityProducts.ForEach(returnList.Add);
                WeightProducts.ForEach(returnList.Add);

                return returnList;
            }
        }

        public static List<ProductByQuantity> QuantityProducts = new List<ProductByQuantity>
        {
            
        };

        public static List<ProductByWeight> WeightProducts = new List<ProductByWeight>
        {
            
        };

        // cart list on the server

        public static List<Product> Cart = new List<Product>
        {

        };
        
        public static int NextId()
        {
                if (!Products.Any())
                {
                    return 1;
                }

                return Products.Select(t => t.Id).Max() + 1;
        }
    }
}
