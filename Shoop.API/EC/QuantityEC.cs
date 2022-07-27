using Library.Shoop.Models;
using Shoop.API.Database;

namespace Shoop.API.EC
{
    public class QuantityEC
    {
        public List<ProductByQuantity> Get()
        {
            return FakeDatabase.QuantityProducts;
        }
    }
}
