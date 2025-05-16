using Npgsql;
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

        public class CartItem : INotifyPropertyChanged
        {
            public required string Name { get; set; }

            private double _price;
            public double Price
            {
                get => _price;
                set
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }

            private int _quantity;
            public int Quantity
            {
                get => _quantity;
                set
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }

            public double TotalPrice => Price * Quantity;

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadProductsFromDB()
        {
            string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            LoadItems(conn);
            LoadCategorySubcategoryMapping(conn);
            LoadSubcategoryItemMapping(conn);
        }

        private void LoadItems(NpgsqlConnection conn)
        {
            try
            {
                using var cmd = new NpgsqlCommand("SELECT name, price, image_path FROM item", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    decimal price = reader.GetDecimal(1);
                    string imagePath = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    _products[name] = price;
                    _productImages[name] = imagePath;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading _products: " + ex.Message);
                _products["Pizza"] = 400;
                _products["Momo"] = 150;
                _products["Fried Rice"] = 180;
            }
        }

        private void LoadCategorySubcategoryMapping(NpgsqlConnection conn)
        {
            using var cmd = new NpgsqlCommand("SELECT c.name AS category_name, s.name AS subcategory_name FROM subcategory s JOIN category c ON s.category_id = c.id", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string category = reader.GetString(0);
                string subcategory = reader.GetString(1);
                if (!_categoryToSubcategories.ContainsKey(category))
                    _categoryToSubcategories[category] = new List<string>();
                _categoryToSubcategories[category].Add(subcategory);
            }
        }

        private void LoadSubcategoryItemMapping(NpgsqlConnection conn)
        {
            using var cmd = new NpgsqlCommand("SELECT s.name AS subcategory_name, i.name AS item_name FROM item i JOIN subcategory s ON i.subcategory_id = s.id", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string subcategory = reader.GetString(0);
                string item = reader.GetString(1);
                if (!_subcategoryToItems.ContainsKey(subcategory))
                    _subcategoryToItems[subcategory] = new List<string>();
                _subcategoryToItems[subcategory].Add(item);
            }
        }

        private void PopulateCategories()
        {
            ProductFilter.cmbCategory.Items.Clear();
            var categories = new List<string> { "All" };
            categories.AddRange(_categoryToSubcategories.Keys);
            ProductFilter.cmbCategory.ItemsSource = categories;
            ProductFilter.cmbCategory.SelectedIndex = 0;
        }

        public void UpdateCartDisplay()
        {
            var cartItems = _cartQuantities.Select(item => new CartItem
            {
                Name = item.Key,
                Price = (double)_products[item.Key],
                Quantity = item.Value
            }).ToList();

            ShoppingCart.cartListView.ItemsSource = null;
            ShoppingCart.cartListView.ItemsSource = cartItems;

            foreach (var items in ShoppingCart.cartListView.Items)
            {
                Debug.WriteLine(items);
            }

            foreach (var items in cartItems)
            {
                Debug.WriteLine(items);
            }

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = _cartQuantities.Sum(item => _products[item.Key] * item.Value);
            ShoppingCart.lblTotal.Content = $"Total: Rs. {total}";
        }

        private void AddProductToWrapPanel(WrapPanel panel, string name, decimal price)
        {
            string imagePath = GetImagePath(name);

            var productImage = new System.Windows.Controls.Image
            {
                Width = 80,
                Height = 80,
                Margin = new Thickness(5),
                Stretch = Stretch.UniformToFill,
                Source = LoadImage(imagePath)
            };

            var button = new Button
            {
                Content = $"{name}\nRs. {price}",
                Width = 100,
                Height = 50,
                Tag = name,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };
            button.Click += AddToCart_Click;

            var productPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };
            productPanel.Children.Add(productImage);
            productPanel.Children.Add(button);

            panel.Children.Add(productPanel);
        }

        private string GetImagePath(string name)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string basePath = System.IO.Path.Combine(baseDir, "Assets/Images"); //Where image is stored.
            string fileName = _productImages.ContainsKey(name) ? _productImages[name] : name.ToLower().Replace(" ", "_") + ".jpg";
            string imagePath = Path.Combine(basePath, fileName);
            return File.Exists(imagePath) ? imagePath : Path.Combine(basePath, "placeholder.jpg");
        }

        private BitmapImage LoadImage(string path)
        {
            try
            {
                return new BitmapImage(new Uri(path, UriKind.Absolute));
            }
            catch
            {
                return new BitmapImage(new Uri("Images/placeholder.jpg", UriKind.Relative));
            }
        }

        private void DisplayProducts()
        {
            ProductDisplay.flowPanelItems.Children.Clear();

            foreach (var category in _categoryToSubcategories.Keys)
            {
                // Create vertical StackPanel for this category's subcategories
                var categoryStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(5)
                };

                // Add each subcategory as an Expander inside the category's StackPanel
                foreach (var subcategory in _categoryToSubcategories[category])
                {
                    if (!_subcategoryToItems.ContainsKey(subcategory))
                        continue;

                    var subcategoryExpander = new Expander
                    {
                        Header = new TextBlock
                        {
                            Text = subcategory,
                            FontWeight = FontWeights.SemiBold,
                            FontSize = 16,
                            Foreground = Brushes.DarkSlateGray
                        },
                        Margin = new Thickness(10, 5, 10, 5),
                        IsExpanded = true
                    };

                    var itemsPanel = new WrapPanel
                    {
                        Margin = new Thickness(20, 5, 10, 10),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    foreach (var item in _subcategoryToItems[subcategory])
                    {
                        if (_products.ContainsKey(item))
                        {
                            AddProductToWrapPanel(itemsPanel, item, _products[item]);
                        }
                    }

                    subcategoryExpander.Content = itemsPanel;
                    categoryStackPanel.Children.Add(subcategoryExpander);
                }

                // Now add category Expander, containing the subcategory stack
                var categoryExpander = new Expander
                {
                    Header = new TextBlock
                    {
                        Text = category,
                        FontWeight = FontWeights.Bold,
                        FontSize = 18,
                        Foreground = Brushes.DarkSlateBlue
                    },
                    Margin = new Thickness(10, 15, 10, 5),
                    IsExpanded = true,
                    Content = categoryStackPanel
                };

                // Add the full category row to the main vertical panel
                ProductDisplay.flowPanelItems.Children.Add(categoryExpander);
            }
        }

        private void ApplyFilters()
        {
            ProductDisplay.flowPanelItems.Children.Clear();

            try
            {
                string searchText = ProductFilter.txtSearch.Text?.ToLower() ?? string.Empty;
                string selectedCategory = ProductFilter.cmbCategory.SelectedItem as string ?? "All";
                string selectedSubcategory = ProductFilter.cmbSubcategory.SelectedItem as string ?? "All";

                decimal.TryParse(ProductFilter.numMinPrice.Text, out decimal minPrice);
                decimal.TryParse(ProductFilter.numMaxPrice.Text, out decimal maxPrice);

                string tempSortOption = ProductFilter.cmbSort.SelectedValue.ToString() ?? "Default";
                if (tempSortOption != "Default")
                {
                    tempSortOption = tempSortOption.Split(":")[1].Trim();
                }
                string sortOption = tempSortOption;

                // Filter products first
                var filtered = _products
                    .Where(p =>
                        (string.IsNullOrWhiteSpace(searchText) || p.Key.ToLower().Contains(searchText)) &&
                        p.Value >= minPrice && p.Value <= maxPrice
                    )
                    .ToList();

                // Group by Category → Subcategory → Products
                var grouped = new Dictionary<string, Dictionary<string, List<KeyValuePair<string, decimal>>>>();

                foreach (var product in filtered)
                {
                    foreach (var subcatEntry in _subcategoryToItems)
                    {
                        if (!subcatEntry.Value.Contains(product.Key))
                            continue;

                        string subcategory = subcatEntry.Key;

                        // Skip if subcategory doesn't match filter
                        if (selectedSubcategory != "All" && selectedSubcategory != subcategory)
                            continue;

                        // Find category that owns this subcategory
                        var category = _categoryToSubcategories
                            .FirstOrDefault(c => c.Value.Contains(subcategory));

                        if (category.Key == null)
                            continue;

                        // Skip if category doesn't match filter
                        if (selectedCategory != "All" && selectedCategory != category.Key)
                            continue;

                        if (!grouped.ContainsKey(category.Key))
                            grouped[category.Key] = new Dictionary<string, List<KeyValuePair<string, decimal>>>();

                        if (!grouped[category.Key].ContainsKey(subcategory))
                            grouped[category.Key][subcategory] = new List<KeyValuePair<string, decimal>>();

                        grouped[category.Key][subcategory].Add(product);
                    }
                }

                // Sort and render
                foreach (var categoryPair in grouped)
                {
                    var categoryStackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(5)
                    };

                    foreach (var subcategoryPair in categoryPair.Value)
                    {
                        // Fix for CS8506: No best type was found for the switch expression.
                        // The issue occurs because the switch expression needs a consistent type for all cases.
                        // Ensure that all cases in the switch expression return the same type.
                        var sortedProducts = sortOption switch
                        {
                            "Price Low to High" => subcategoryPair.Value.OrderBy(p => p.Value).ToList(),
                            "Price High to Low" => subcategoryPair.Value.OrderByDescending(p => p.Value).ToList(),
                            "A-Z" => subcategoryPair.Value.OrderBy(p => p.Key).ToList(),
                            "Z-A" => subcategoryPair.Value.OrderByDescending(p => p.Key).ToList(),
                            _ => subcategoryPair.Value.ToList()
                        };

                        var itemsPanel = new WrapPanel
                        {
                            Margin = new Thickness(20, 5, 10, 10),
                            HorizontalAlignment = HorizontalAlignment.Left
                        };

                        foreach (var product in sortedProducts)
                        {
                            AddProductToWrapPanel(itemsPanel, product.Key, product.Value);
                        }

                        var subcategoryExpander = new Expander
                        {
                            Header = new TextBlock
                            {
                                Text = subcategoryPair.Key,
                                FontWeight = FontWeights.SemiBold,
                                FontSize = 16,
                                Foreground = Brushes.DarkSlateGray
                            },
                            Margin = new Thickness(10, 5, 10, 5),
                            IsExpanded = true,
                            Content = itemsPanel
                        };

                        categoryStackPanel.Children.Add(subcategoryExpander);
                    }

                    var categoryExpander = new Expander
                    {
                        Header = new TextBlock
                        {
                            Text = categoryPair.Key,
                            FontWeight = FontWeights.Bold,
                            FontSize = 18,
                            Foreground = Brushes.DarkSlateBlue
                        },
                        Margin = new Thickness(10, 15, 10, 5),
                        IsExpanded = true,
                        Content = categoryStackPanel
                    };

                    ProductDisplay.flowPanelItems.Children.Add(categoryExpander);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Filter error: " + ex);
            }
        }

        public OrderPage()
        {
            InitializeComponent();
            
        }

        public OrderPage(MainWindow mainWindow, String role)
        {
            InitializeComponent();
            LoadProductsFromDB();
            PopulateCategories();
            DisplayProducts();
            _mainWindow = mainWindow;
            _userSession = role;
            Loaded += (s, e) => _isInitialized = true;

            //Shopping Cart Handlers
            ShoppingCart.btnCheckout.Click += btnCheckout_Click;
            ShoppingCart.btnClear.Click += clear_Click;
            ShoppingCart.RemoveRequested += cartRemove;
            ShoppingCart.QuantityUpdated += quantityUpdate;


            //Product Filter Handlers
            ProductFilter.txtSearch.TextChanged += txtSearch_TextChanged;
            ProductFilter.cmbCategory.SelectionChanged += cmbCategory_SelectionChanged;
            ProductFilter.cmbSubcategory.SelectionChanged += cmbSubcategory_SelectionChanged;
            ProductFilter.cmbSort.SelectionChanged += cmbSort_SelectionChanged;
            ProductFilter.numMinPrice.TextChanged += numMinPrice_TextChanged;
            ProductFilter.numMaxPrice.TextChanged += numMaxPrice_TextChanged;
            ProductFilter.btnReset.Click += btnReset_Click;
            ProductFilter.numMaxPriceUp.Click += MaxPriceUp_Click;
            ProductFilter.numMaxPriceDown.Click += MaxPriceDown_Click;
            ProductFilter.numMinPriceUp.Click += MinPriceUp_Click;
            ProductFilter.numMinPriceDown.Click += MinPriceDown_Click;
            ProductFilter.btnReset.Click += btnReset_Click;
        }

        private void quantityUpdate(ComboBox comboBox)
        {
            if(comboBox.DataContext is CartItem item)
            {
                int newQuantity = (int)comboBox.SelectedItem;

                // Avoid infinite recursion by only updating when there's a change
                if (_cartQuantities[item.Name] != newQuantity)
                {
                    _cartQuantities[item.Name] = newQuantity;
                    UpdateCartDisplay();
                }
            }
        }

        private void cartRemove(Button button)
        {
            if(button.DataContext is CartItem item)
            {
                _cartQuantities.Remove(item.Name);
                UpdateCartDisplay();
            }
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

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            _cartQuantities.Clear();
            UpdateCartDisplay();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
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
            if (_isInitialized)
                ApplyFilters();
        }

        private void cmbSubcategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitialized)
                ApplyFilters();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fix for CS0021: Cannot apply indexing with [] to an expression of type 'ComboBox'
            if (ProductFilter.cmbSort.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortOption = selectedItem.Content.ToString() ?? "Default";
                Debug.WriteLine(sortOption);
            }

            if (_isInitialized)
                ApplyFilters();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string productName)
            {
                if (!_products.ContainsKey(productName))
                {
                    MessageBox.Show("Product not found.");
                    return;
                }

                // Increase quantity or add new
                if (_cartQuantities.ContainsKey(productName))
                    _cartQuantities[productName]++;
                else
                    _cartQuantities[productName] = 1;
                foreach(var cart in _cartQuantities)
                {
                    Debug.WriteLine(cart);
                }
                
                UpdateCartDisplay();
            }
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
            if (_isInitialized)
                ApplyFilters();
        }

        private void numMaxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isInitialized)
                ApplyFilters();
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
