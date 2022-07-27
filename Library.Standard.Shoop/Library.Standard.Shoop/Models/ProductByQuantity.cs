﻿using Library.Standard.Shoop.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Models
{

    [JsonConverter(typeof(ProductJsonConverter))]

    public partial class ProductByQuantity : Product
    {
        public override int typeOfProduct { get; set; }

        public int Quantity { get; set; }

        public bool Bogo { get; set; }

        public override double TotalPrice
        {
            get
            {
                return Price * typeOfProduct;
            }
        }

        public override string ToString()
        {
            return $"{Id} - {Name} - {Description} - ${Price} - {typeOfProduct} - ${TotalPrice} - IsBogo: {IsBogo}";
        }

    }
}
