using Npgsql;
using POS.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Path = System.IO.Path;

namespace POS
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {

        private MainWindow _mainWindow;

        // Fields for the OrderPage class
        private bool _isInitialized = false;

        private bool _windowChange = false; // Flag to track if the back button was clicked
        private string _userSession = String.Empty;
        private readonly Dictionary<string, decimal> _products = new();
        private readonly Dictionary<string, string> _productImages = new();
        private readonly Dictionary<string, List<string>> _categoryToSubcategories = new();
        private readonly Dictionary<string, List<string>> _subcategoryToItems = new();
        public readonly Dictionary<string, int> _cartQuantities = new();


        private void ProductFilterControl_FilterChanged(object? sender, FilterChangedEventArgs e)
        {
            ProductDisplay.ApplyFilter(e);
        }

        private void OnProductAddedToCart(Product product)
        {
            ShoppingCart.AddProductToCart(product);
        }

        public OrderPage()
        {
            //InitializeComponent();
            
        }

        public OrderPage(MainWindow mainWindow, String role)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _userSession = role;
            Loaded += (s, e) => _isInitialized = true;

            ProductDisplay.ProductAddedToCart += OnProductAddedToCart;

            ProductFilter.RefreshCategories();
            ProductFilter.FilterChanged += ProductFilterControl_FilterChanged;
        }


        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (_cartQuantities.Count == 0)
            {
                MessageBox.Show("Your cart is empty.");
                return;
            }

            // Show the receipt window
            _mainWindow.MainFrame.Navigate(new ReceiptPreviewPage(_cartQuantities, _products));
            //ReceiptPrintWindow receiptWindow = new ReceiptPrintWindow(_cartQuantities, _products);
            //receiptWindow.Show();
            //receiptWindow.printButton.Click += (s, args) =>
            //{
            //    // Optionally handle any actions after the print button is clicked
            //    // For example, you can navigate back to the main order window
            //    // Optionally reset the cart
            //    _cartQuantities.Clear();
            //    UpdateCartDisplay();
            //};

            //receiptWindow.Closed += (s, args) =>
            //{
            //    // Optionally handle any actions after the receipt window is closed
            //    // For example, you can navigate back to the main order window
            //    this.Show();
            //    return;
            //};

        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            //This is new order (Disabled button)
        }

        private void ViewOrder_Click(object sender, RoutedEventArgs e)
        {
            //NavigateToViewOrder();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ApplyFilters();
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductFilter.cmbSubcategory.ItemsSource = null;
            ProductFilter.cmbSubcategory.Items.Clear();
            var subCategories = new List<string> { "All" };
            if (ProductFilter.cmbCategory.SelectedItem is string selectedCategory)
            {
                if (selectedCategory == "All")
                {
                    subCategories.AddRange(_subcategoryToItems.Keys);
                }
                else if (_categoryToSubcategories.ContainsKey(selectedCategory))
                {
                    // Show only subcategories under the selected category
                    subCategories.AddRange(_categoryToSubcategories[selectedCategory]);
                }
                ProductFilter.cmbSubcategory.ItemsSource = subCategories;
                ProductFilter.cmbSubcategory.SelectedIndex = 0;
            }
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void cmbSubcategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fix for CS0021: Cannot apply indexing with [] to an expression of type 'ComboBox'
            if (ProductFilter.cmbSort.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortOption = selectedItem.Content.ToString() ?? "Default";
                Debug.WriteLine(sortOption);
            }

            //if (_isInitialized)
            //    ApplyFilters();
        }



        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            //NavigateBackToLogin();
        }

        private void MaxPriceUp_Click(object sender, RoutedEventArgs e)
        {

            ProductFilter.numMaxPrice.Text = (Convert.ToDecimal(ProductFilter.numMaxPrice.Text) + 1).ToString();
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void MaxPriceDown_Click(object sender, RoutedEventArgs e)
        {
            ProductFilter.numMaxPrice.Text = (Convert.ToDecimal(ProductFilter.numMaxPrice.Text) - 1).ToString();
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void MinPriceUp_Click(object sender, RoutedEventArgs e)
        {
            ProductFilter.numMinPrice.Text = (Convert.ToDecimal(ProductFilter.numMinPrice.Text) + 1).ToString();
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void MinPriceDown_Click(object sender, RoutedEventArgs e)
        {
            ProductFilter.numMinPrice.Text = (Convert.ToDecimal(ProductFilter.numMinPrice.Text) - 1).ToString();
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void numMinPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void numMaxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (_isInitialized)
            //    ApplyFilters();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ProductFilter.numMaxPrice.Text = "10000";
            ProductFilter.numMinPrice.Text = "0";
            ProductFilter.cmbCategory.SelectedIndex = 0;
            ProductFilter.cmbSubcategory.SelectedIndex = 0;
            ProductFilter.cmbSort.SelectedIndex = 0;
            ProductFilter.txtSearch.Text = string.Empty;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!_windowChange)
                App.Current.Shutdown();
        }

        private void cartListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
