using Library.Shoop.Models;
using Microsoft.AspNetCore.Mvc;
using Shoop.API.Database;
using Shoop.API.EC;

namespace Shoop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public List<Product> Get()
        {
            return FakeDatabase.Products;
        }

        [HttpGet("Delete/{id}")]
        public int Delete(int id)
        {
            var productToDelete = FakeDatabase.Products.FirstOrDefault(i => i.Id == id);

            if (productToDelete != null)
            {
                var prod = productToDelete as ProductByQuantity;
                var prod2 = productToDelete as ProductByWeight;
                if (prod != null)
                {
                    FakeDatabase.QuantityProducts.Remove(prod);
                    
                }
                else if(prod2 != null)
                {
                    FakeDatabase.WeightProducts.Remove(prod2);
                }
            }

            return id;
        }

    }
}
