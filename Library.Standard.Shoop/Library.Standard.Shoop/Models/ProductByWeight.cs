using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Models
{
    public partial class ProductByWeight : Product
    {
        public override int typeOfProduct { get; set; }

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
            return $"{Id} - {Name} - {Description} - ${Price} - {typeOfProduct}LB - ${TotalPrice} - IsBogo: {IsBogo}";
        }
    }
}
