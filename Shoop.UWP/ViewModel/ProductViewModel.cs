using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shoop.UWP.ViewModel
{
    internal class ProductViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
        public int Id { get; set; }

        public bool IsBogo { get; set; }
        public virtual double TotalPrice { get; set; }

        public int productAmount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
