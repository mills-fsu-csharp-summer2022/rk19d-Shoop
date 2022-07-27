﻿using Library.Standard.Shoop.Utility;
using Newtonsoft.Json;

namespace Library.Shoop.Models // Note: actual namespace depends on the project name.
{

    [JsonConverter(typeof(ProductJsonConverter))]

    public partial class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
        public int Id { get; set; }

        public bool IsBogo { get; set; }
        public virtual double TotalPrice { get; set; }

        public virtual int typeOfProduct { get; set; }

        public int productAmount { get; set; }

        //public Product(ProductViewModel pvm)
        //{
        //    Name = "";
        //    Description = "";
        //}
    }
}