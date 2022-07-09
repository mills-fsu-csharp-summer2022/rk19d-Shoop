using Shoop.UWP.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Shoop.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class cart : Page
    {
        public cart()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).CartRefresh();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Load();
        }

        private void Delete_Cart(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as MainViewModel;

            if (dc != null)
            {
                dc.DeleteCart();
            }
        }
        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as MainViewModel;

            if (dc != null)
            {
                dc.Checkout();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
