using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using POS.Models.Core;

namespace POS
{
    /// <summary>
    /// Interaction logic for ProductFilterControl.xaml
    /// </summary>
    public partial class ProductFilterControl : UserControl
    {
        public event EventHandler<FilterChangedEventArgs> FilterChanged;

        private bool isLoaded = false;
        public List<string> SortOptions { get; set; } = new List<string>
        {
            "Default",
            "Price Low to High",
            "Price High to Low",
            "A-Z",
            "Z-A"
        };
        private ObservableCollection<string> categories = new ObservableCollection<string>();
        private ObservableCollection<string> subCategories = new ObservableCollection<string>();

        private void PopulateCategories()
        {
            cmbCategory.ItemsSource = null;
            categories.Clear();
            categories.Add("All");
            foreach (var c in ProductDataStore.Categories.Select(c => c.Name))
                categories.Add(c);

            cmbCategory.ItemsSource = categories;
            cmbCategory.SelectedIndex = 0;
            cmbCategory.UpdateLayout();
            cmbCategory.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render); // Force render

            PopulateSubcategories(null);
        }

        private void PopulateSubcategories(string? categoryName)
        {
            cmbSubcategory.ItemsSource = null;
            subCategories.Clear();
            subCategories.Add("All");

            if (!string.IsNullOrWhiteSpace(categoryName) && categoryName != "All")
            {
                var category = ProductDataStore.GetCategory(categoryName);
                if (category != null)
                {
                    foreach (var c in category.Subcategories.Select(c => c.Name))
                        subCategories.Add(c);
                }
                else
                {
                    Debug.WriteLine("Category not found or null");
                }
            }
            

            cmbSubcategory.ItemsSource = subCategories;
            cmbSubcategory.SelectedIndex = 0;
            cmbSubcategory.UpdateLayout();
            cmbSubcategory.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void OnFilterChanged()
        {
            if (!isLoaded) return;

            var args = new FilterChangedEventArgs
            {
                SelectedCategory = cmbCategory.SelectedItem as string ?? "All",
                SelectedSubcategory = cmbSubcategory.SelectedItem as string ?? "All",
                MinPrice = double.TryParse(numMinPrice.Text, out var min) ? min : (double?)null,
                MaxPrice = double.TryParse(numMaxPrice.Text, out var max) ? max : (double?)null,
                SortOption = cmbSort.SelectedItem as string ?? "Default"
            };

            FilterChanged?.Invoke(this, args);
        }

        public ProductFilterControl()
        {
            InitializeComponent();
            DataContext = this; // crucial for binding SortOptions
            Loaded += ProductFilterControl_Loaded;
        }

        public void RefreshCategories()
        {
            PopulateCategories();
        }

        private void ProductFilterControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateCategories();
            isLoaded = true;
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedCategory = cmbCategory.SelectedItem as string;
            PopulateSubcategories(selectedCategory);
            if(isLoaded)
                OnFilterChanged();
        }

        private void cmbSubcategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }

        private void numMinPriceUp_Click(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }

        private void numMinPriceDown_Click(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }

        private void numMinPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }

        private void numMaxPriceUp_Click(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }

        private void numMaxPriceDown_Click(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }

        private void numMaxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isLoaded)
                OnFilterChanged();
        }
    }
    public class FilterChangedEventArgs : EventArgs
    {
        public string SelectedCategory { get; set; }
        public string SelectedSubcategory { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string SortOption { get; set; }
    }

}
