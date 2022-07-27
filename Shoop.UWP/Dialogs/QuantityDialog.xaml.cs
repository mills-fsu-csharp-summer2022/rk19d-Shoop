using Library.Shoop.Models;
using Library.Shoop.Services;
using Shoop.UWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Shoop.UWP.Dialogs
{
    public sealed partial class QuantityDialog : ContentDialog
    {      
        public QuantityDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByQuantity();
        }

        public QuantityDialog(Product selectedItem)
        {
            this.InitializeComponent();
            this.DataContext = selectedItem;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            AdminService.Current.AddOrUpdate(DataContext as ProductByQuantity);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
