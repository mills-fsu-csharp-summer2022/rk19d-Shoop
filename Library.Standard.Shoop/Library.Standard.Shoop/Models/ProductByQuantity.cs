using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Models
{
    public partial class ProductByQuantity : Product
    {

        public int Quantity { get; set; }

        public override double TotalPrice
        {
            get
            {
                return Price * Quantity;
            }
        }

        public override string ToString()
        {
            return $"{Id} - {Name} - {Description} - ${Price} - {Quantity} - ${TotalPrice} - IsBogo: {IsBogo}";
        }

    }
}
