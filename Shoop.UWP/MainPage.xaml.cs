using Shoop.UWP.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Shoop.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
        }

        private async void Add_Quantity_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as MainViewModel;
            
            if (dc != null)
            {
               await dc.Add(ProductType.Quantity);
            }
        }

        private async void Add_Weight_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as MainViewModel;

            if (dc != null)
            {
                await dc.Add(ProductType.Weight);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as MainViewModel;

            if (dc != null)
            {
                dc.Delete();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as MainViewModel;

            if (dc != null)
            {
                dc.Update();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Load();
        }

        private void AddCart_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as MainViewModel;

            if (dc != null)
            {
                dc.AddToCart();
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(cart));
        }
    }
}
