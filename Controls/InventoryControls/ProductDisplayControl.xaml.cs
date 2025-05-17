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
using POS.ViewModels.Products;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;


namespace POS
{
    /// <summary>
    /// Interaction logic for ProductDisplayControl.xaml
    /// </summary>
    public partial class ProductDisplayControl : UserControl
    {

        public ProductDisplayViewModel ViewModel { get; } = new();

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<CategoryGroup>), typeof(ProductDisplayControl), new PropertyMetadata(null));

        public IEnumerable<CategoryGroup> ItemsSource
        {
            get => (IEnumerable<CategoryGroup>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty AddToCartCommandProperty =
            DependencyProperty.Register(nameof(AddToCartCommand), typeof(ICommand), typeof(ProductDisplayControl), new PropertyMetadata(null));

        public ICommand AddToCartCommand
        {
            get => (ICommand)GetValue(AddToCartCommandProperty);
            set => SetValue(AddToCartCommandProperty, value);
        }

        public ProductDisplayControl()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += ProductDisplayControl_Loaded;
        }

        private void ProductDisplayControl_Loaded(object sender, RoutedEventArgs e)
        {
            ItemsSource = ViewModel.GroupedProducts;
        }

        public void RefreshDisplay(IEnumerable<Product> filtered)
        {
            ViewModel.UpdateGroupedProducts(filtered);
            ItemsSource = ViewModel.GroupedProducts;
        }
    }
}