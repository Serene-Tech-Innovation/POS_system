using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using POS.Models.Core;


namespace POS
{
    /// <summary>
    /// Interaction logic for ProductDisplayControl.xaml
    /// </summary>
    public partial class ProductDisplayControl : UserControl
    {
        // ItemsSource DP for binding the product list
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<Product>),
                typeof(ProductDisplayControl));

        public IEnumerable<Product> ItemsSource
        {
            get => (IEnumerable<Product>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // Command DP for adding items to cart
        public static readonly DependencyProperty AddToCartCommandProperty =
            DependencyProperty.Register(
                nameof(AddToCartCommand),
                typeof(ICommand),
                typeof(ProductDisplayControl));

        public ICommand AddToCartCommand
        {
            get => (ICommand)GetValue(AddToCartCommandProperty);
            set => SetValue(AddToCartCommandProperty, value);
        }

        public ProductDisplayControl()
        {
            InitializeComponent();
        }

        // Wire the Add button inside each row to this handler
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Product p)
            {
                if (AddToCartCommand?.CanExecute(p) == true)
                    AddToCartCommand.Execute(p);
            }
        }
    }
}