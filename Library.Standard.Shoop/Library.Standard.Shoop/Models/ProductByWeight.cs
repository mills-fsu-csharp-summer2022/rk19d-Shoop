using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Models
{
    public partial class ProductByWeight : Product
    {
        public double Weight { get; set; }

        public override double TotalPrice
        {
            get
            {
                return Price * Weight;
            }
        }

        public override string ToString()
        {
            return $"{Id} - {Name} - {Description} - ${Price} - {Weight}LB - ${TotalPrice} - IsBogo: {IsBogo}";
        }
    }
}
