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
        public event Action<Product> ProductAddedToCart;

        private readonly Dictionary<string, string> _productImages = new();

        private void DisplayProducts()
        {
            flowPanelItems.Children.Clear();

            foreach (var category in ProductDataStore.Categories)
            {
                var categoryStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(5)
                };

                foreach (var subcategory in category.Subcategories)
                {
                    var items = ProductDataStore.Products
                        .Where(p => p.Subcategory?.Name == subcategory.Name)
                        .ToList();

                    if (!items.Any()) continue;

                    var subcategoryExpander = new Expander
                    {
                        Header = new TextBlock
                        {
                            Text = subcategory.Name,
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

                    foreach (var product in items)
                    {
                        AddProductToWrapPanel(itemsPanel, product);
                    }

                    subcategoryExpander.Content = itemsPanel;
                    categoryStackPanel.Children.Add(subcategoryExpander);
                }

                var categoryExpander = new Expander
                {
                    Header = new TextBlock
                    {
                        Text = category.Name,
                        FontWeight = FontWeights.Bold,
                        FontSize = 18,
                        Foreground = Brushes.DarkSlateBlue
                    },
                    Margin = new Thickness(10, 15, 10, 5),
                    IsExpanded = true,
                    Content = categoryStackPanel
                };

                flowPanelItems.Children.Add(categoryExpander);
            }
        }

        private void AddProductToWrapPanel(WrapPanel panel, Product product)
        {
            string imagePath = GetImagePath(product.Name);

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
                Content = $"{product.Name}\nRs. {product.Price}",
                Width = 100,
                Height = 50,
                Tag = product.Name,
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

        public void ApplyFilter(FilterChangedEventArgs filter)
        {
            var filteredProducts = ProductDataStore.Products
                .Where(p =>
                    (filter.SelectedCategory == "All" || p.Category?.Name == filter.SelectedCategory) &&
                    (filter.SelectedSubcategory == "All" || p.Subcategory?.Name == filter.SelectedSubcategory) &&
                    (!filter.MinPrice.HasValue || p.Price >= (decimal)filter.MinPrice.Value) &&
                    (!filter.MaxPrice.HasValue || p.Price <= (decimal)filter.MaxPrice.Value))
                .ToList();

            if(filter.TextSearch != null)
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Name.IndexOf(filter.TextSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            // Apply sorting
            filteredProducts = filter.SortOption switch
            {
                "Price Low to High" => filteredProducts.OrderBy(p => p.Price).ToList(),
                "Price High to Low" => filteredProducts.OrderByDescending(p => p.Price).ToList(),
                "A-Z" => filteredProducts.OrderBy(p => p.Name).ToList(),
                "Z-A" => filteredProducts.OrderByDescending(p => p.Name).ToList(),
                _ => filteredProducts
            };

            DisplayFilteredProducts(filteredProducts);
        }
        
        private void DisplayFilteredProducts(List<Product> products)
        {
            flowPanelItems.Children.Clear();

            var grouped = products
                .GroupBy(p => p.Category?.Name)
                .OrderBy(g => g.Key);

            foreach (var categoryGroup in grouped)
            {
                var categoryStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(5)
                };

                var subGrouped = categoryGroup
                    .GroupBy(p => p.Subcategory?.Name)
                    .OrderBy(g => g.Key);

                foreach (var subGroup in subGrouped)
                {
                    var itemsPanel = new WrapPanel
                    {
                        Margin = new Thickness(20, 5, 10, 10),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    foreach (var product in subGroup)
                        AddProductToWrapPanel(itemsPanel, product);

                    var subcategoryExpander = new Expander
                    {
                        Header = new TextBlock
                        {
                            Text = subGroup.Key ?? "Unknown Subcategory",
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
                        Text = categoryGroup.Key ?? "Unknown Category",
                        FontWeight = FontWeights.Bold,
                        FontSize = 18,
                        Foreground = Brushes.DarkSlateBlue
                    },
                    Margin = new Thickness(10, 15, 10, 5),
                    IsExpanded = true,
                    Content = categoryStackPanel
                };

                flowPanelItems.Children.Add(categoryExpander);
            }
        }

        public ProductDisplayControl()
        {
            InitializeComponent();
            
            ProductDataStore.ProductUpdated += (product) =>
            {
                DisplayProducts();
                // Update the product image path if it has changed
            };
            ProductDataStore.CategoryUpdated += Category =>
            {
                DisplayProducts();
                // Refresh the display if needed
            };
            ProductDataStore.SubcategoryUpdated += Subcategory =>
            {
                DisplayProducts();
                // Refresh the display if needed
            };

            DisplayProducts();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string productName)
            {
                var product = ProductDataStore.GetProduct(productName);
                if (product == null)
                {
                    MessageBox.Show("Product not found.");
                    return;
                }

                // Raise the event instead of showing MessageBox
                ProductAddedToCart?.Invoke(product);
            }
        } 
    }
}
