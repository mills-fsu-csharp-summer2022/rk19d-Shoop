using Library.Shoop.Models;
using Microsoft.AspNetCore.Mvc;
using Shoop.API.Database;
using Shoop.API.EC;

namespace Shoop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public List<Product> Get()
        {
            return FakeDatabase.Cart;
        }
        
        [HttpPost("AddToCart")]
        public Product Add(Product q)
        {
            
            var productToAdd = FakeDatabase.Products.FirstOrDefault(t => t.Id == q.Id);
            if (productToAdd != null)
            {
                FakeDatabase.Cart.Add(productToAdd);
            }
            
            return q;
        }

        [HttpGet("Delete/{id}")]

        public int Delete(int id)
        {
            var productToDelete = FakeDatabase.Cart.FirstOrDefault(i => i.Id == id);

            if (productToDelete != null)
            {

                FakeDatabase.Products.Add(productToDelete);
                FakeDatabase.Cart.Remove(productToDelete);
            }

            return id;
        }
    }
}
