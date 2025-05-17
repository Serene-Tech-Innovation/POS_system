using POS.Models.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS.ViewModels.Products
{
    public class ProductFilterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        ProductDisplayViewModel? displayVM = App.Current.MainWindow?.DataContext as ProductDisplayViewModel;

        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Subcategory> subCategories { get; set; } = new();
        public ObservableCollection<string> SortOptions { get; set; } = new()
        {
            "Price: Low to High",
            "Price: High to Low",
            "Name: A to Z",
            "Name: Z to A"
        };

        public ObservableCollection<Product> FilteredProducts { get; set; } = new();

        private ObservableCollection<Product> _allProducts = new();

        public ProductFilterViewModel()
        {
            // Load all products from your DataStore or viewmodel
            _allProducts = new ObservableCollection<Product>(ProductDataStore.Products);
            Categories = new ObservableCollection<Category>(ProductDataStore.Categories);
            subCategories = new ObservableCollection<Subcategory>(ProductDataStore.Categories.SelectMany(c => c.Subcategories));
            //subCategories = ProductDataStore.Categories.SelectMany(c => c.Subcategories).ToList();
            // Initially all products shown
            FilteredProducts = new ObservableCollection<Product>(_allProducts);
        }

        public void ApplyFilter(FilterChangedEventArgs e)
        {
            if (e == null) return;

            var filtered = _allProducts.AsEnumerable();
            Debug.WriteLine($"Applying filter: {e.TextSearch}, {e.SelectedCategory}, {e.SelectedSubcategory}, {e.MinPrice}, {e.MaxPrice}, {e.SortOption}");

            // Filter by search text
            if (!string.IsNullOrWhiteSpace(e.TextSearch))
            {
                var searchLower = e.TextSearch.ToLower();
                filtered = filtered.Where(p => p.Name.ToLower().Contains(searchLower));
            }

            // Filter by category
            if (!string.IsNullOrEmpty(e.SelectedCategory))
            {
                filtered = filtered.Where(p => p.Category.Name == e.SelectedCategory);
            }

            // Filter by subcategory
            if (!string.IsNullOrEmpty(e.SelectedSubcategory))
            {
                filtered = filtered.Where(p => p.Subcategory.Name == e.SelectedSubcategory);
            }

            // Filter by price
            if (e.MinPrice.HasValue)
            {
                filtered = filtered.Where(p => (double)p.Price >= e.MinPrice.Value);
            }
            if (e.MaxPrice.HasValue)
            {
                filtered = filtered.Where(p => (double)p.Price <= e.MaxPrice.Value);
            }

            // Sorting
            filtered = e.SortOption switch
            {
                "Price: Low to High" => filtered.OrderBy(p => p.Price),
                "Price: High to Low" => filtered.OrderByDescending(p => p.Price),
                "Name: A to Z" => filtered.OrderBy(p => p.Name),
                "Name: Z to A" => filtered.OrderByDescending(p => p.Name),
                _ => filtered
            };

            // Update FilteredProducts collection
            FilteredProducts.Clear();
            foreach (var p in filtered)
                FilteredProducts.Add(p);

            OnPropertyChanged(nameof(FilteredProducts));
            displayVM?.UpdateGroupedProducts(filtered);
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
