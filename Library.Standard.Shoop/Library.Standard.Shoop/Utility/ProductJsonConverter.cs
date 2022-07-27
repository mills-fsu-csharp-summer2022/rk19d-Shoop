using Library.Shoop.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Standard.Shoop.Utility
{
    public class ProductJsonConverter : JsonCreationConverter<Product>
    {
        protected override Product Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["Quantity"] != null || jObject["quantity"] != null)
            {
                return new ProductByQuantity();
            }
            else if (jObject["Weight"] != null || jObject["weight"] != null)
            {
                return new ProductByWeight();
            }
            else
            {
                return new Product();
            }
        }
    }
}
