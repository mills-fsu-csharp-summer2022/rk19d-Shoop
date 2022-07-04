using Library.Shoop.Models;
using Library.Shoop.Services;
using Shoop.UWP.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Shoop.UWP.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public string Query { get; set; }
        public Product SelectedProduct { get; set; }

        private AdminService _productService;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Product> Products
        {
            get
            {
                if (_productService == null)
                {
                    return new ObservableCollection<Product>();
                }

                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<Product>(_productService.Inventory);
                }
                else
                {
                    return new ObservableCollection<Product>(_productService.Inventory.Where(i => i.Name.ToLower().Contains(Query.ToLower())
                        || i.Description.ToLower().Contains(Query.ToLower())
                        ));

                }
            }
        }

        public MainViewModel()
        {
            _productService = AdminService.Current;
        }

        public async Task Add(ProductType pType)
        {

            ContentDialog diag = null;

            if (pType == ProductType.Quantity)
            {
                diag = new QuantityDialog();
            }
            else if (pType == ProductType.Weight)
            {
                diag = new WeightDialog();
            }
            else
            {
                throw new NotImplementedException();
            }

            
            await diag.ShowAsync();
            NotifyPropertyChanged("Products");
        }
        
        public void Delete()
        {

            var id = SelectedProduct?.Id ?? -1;     
            
            if (id >= 1)
            {
                _productService.Remove(SelectedProduct.Id);
            }

            NotifyPropertyChanged("Products");
        }

        public async void Update()
        {
            if (SelectedProduct != null)
            {
                var diag = new QuantityDialog(SelectedProduct);
                await diag.ShowAsync();
                NotifyPropertyChanged("Products");
            }
        }

        public void Save()
        {

            _productService.Save();
        }

        public void Load()
        {

            _productService.Load();
            NotifyPropertyChanged("Products");
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Products");
        }

        
        
    }
    public enum ProductType
    {
        Quantity,
        Weight
    }
}
