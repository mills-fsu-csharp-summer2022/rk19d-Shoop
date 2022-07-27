using Library.Shoop.Models;
using Microsoft.AspNetCore.Mvc;
using Shoop.API.Database;
using Shoop.API.EC;

namespace Shoop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuantityController : ControllerBase
    {
        private readonly ILogger<QuantityController> _logger;

        public QuantityController(ILogger<QuantityController> logger)
        {
            _logger = logger;
        
        }

        [HttpGet]
        public List<ProductByQuantity> Get()
        {
            return new QuantityEC().Get();
        }

        [HttpPost("AddOrUpdateQuantity")]
        public ProductByQuantity AddOrUpdate(ProductByQuantity q)
        {
            if (q.Id <= 0)
            {
                q.Id = FakeDatabase.NextId();
                FakeDatabase.QuantityProducts.Add(q);
            }

            var productToUpdate = FakeDatabase.QuantityProducts.FirstOrDefault(t => t.Id == q.Id);
            if (productToUpdate != null)
            {
                FakeDatabase.QuantityProducts.Remove(productToUpdate);
                FakeDatabase.QuantityProducts.Add(q);
            }

            return q;
        }

        //[HttpGet("Delete/{id}")]
        //public int Delete(int id)
        //{
        //    var productToDelete = FakeDatabase.Products.FirstOrDefault(i => i.Id == id);

        //    if (productToDelete != null)
        //    {
        //        var prod = productToDelete as ProductByQuantity;
        //        if (prod != null)
        //        {
        //            FakeDatabase.QuantityProducts.Remove(prod);
        //        }
        //    }

        //    return id;
        //}
    }
}
