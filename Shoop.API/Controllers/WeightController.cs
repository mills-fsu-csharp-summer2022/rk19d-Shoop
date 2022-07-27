using Library.Shoop.Models;
using Microsoft.AspNetCore.Mvc;
using Shoop.API.Database;
using Shoop.API.EC;

namespace Shoop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeightController : ControllerBase
    {
        private readonly ILogger<WeightController> _logger;

        public WeightController(ILogger<WeightController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public List<ProductByWeight> Get()
        {
            return FakeDatabase.WeightProducts;
        }



        [HttpPost("AddOrUpdateWeight")]
        public ProductByWeight AddOrUpdate(ProductByWeight q)
        {
            if (q.Id <= 0)
            {
                q.Id = FakeDatabase.NextId();
                FakeDatabase.WeightProducts.Add(q);
            }

            var productToUpdate = FakeDatabase.WeightProducts.FirstOrDefault(t => t.Id == q.Id);
            if (productToUpdate != null)
            {
                FakeDatabase.WeightProducts.Remove(productToUpdate);
                FakeDatabase.WeightProducts.Add(q);
            }

            return q;
        }
    }
}
