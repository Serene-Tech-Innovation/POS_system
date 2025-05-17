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
using POS.ViewModels.Products;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace POS
{
    /// <summary>
    /// Interaction logic for ProductFilterControl.xaml
    /// </summary>
    public partial class ProductFilterControl : UserControl
    {
        private ProductDisplayViewModel DisplayVM
            => DataContext as ProductDisplayViewModel ?? throw new InvalidOperationException();
        public ProductFilterViewModel ViewModel { get; } = new();
        public ProductFilterControl()
        {
            InitializeComponent();
            DataContext = this;

            // Hook event handlers for UI changes:
            txtSearch.TextChanged += OnFilterChanged;
            cmbCategory.SelectionChanged += OnCategoryChanged;
            cmbSubcategory.SelectionChanged += OnFilterChanged;
            cmbSort.SelectionChanged += OnFilterChanged;

            numMinPriceUp.Click += (s, e) => ChangePrice(numMinPrice, 1);
            numMinPriceDown.Click += (s, e) => ChangePrice(numMinPrice, -1);
            numMinPrice.TextChanged += OnFilterChanged;

            numMaxPriceUp.Click += (s, e) => ChangePrice(numMaxPrice, 1);
            numMaxPriceDown.Click += (s, e) => ChangePrice(numMaxPrice, -1);
            numMaxPrice.TextChanged += OnFilterChanged;

            btnReset.Click += (s, e) => ResetFilters();

            Loaded += ProductFilterControl_Loaded;
        }

        private void ProductFilterControl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(ViewModel.Categories.Count);
            cmbCategory.ItemsSource = ViewModel.Categories;
            cmbSubcategory.ItemsSource = ViewModel.subCategories;
            cmbSort.ItemsSource = ViewModel.SortOptions;
        }

        private void ChangePrice(TextBox txtBox, int delta)
        {
            if (double.TryParse(txtBox.Text, out double val))
            {
                val += delta;
                if (val < 0) val = 0;
                txtBox.Text = val.ToString();
            }
            else
            {
                txtBox.Text = "0";
            }
        }

        private void ResetFilters()
        {
            txtSearch.Text = "";
            numMinPrice.Text = "0";
            numMaxPrice.Text = "10000";
            cmbCategory.SelectedIndex = -1;
            cmbSubcategory.ItemsSource = null;
            cmbSubcategory.SelectedIndex = -1;
            cmbSort.SelectedIndex = 0;
            RaiseFilterChanged();
        }

        private void OnCategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCategory.SelectedItem is Category selectedCategory)
            {
                cmbSubcategory.ItemsSource = selectedCategory.Subcategories;
                cmbSubcategory.DisplayMemberPath = "Name";
                cmbSubcategory.SelectedIndex = -1;
            }
            else
            {
                cmbSubcategory.ItemsSource = null;
            }
            RaiseFilterChanged();
        }

        private void OnFilterChanged(object? sender, RoutedEventArgs e)
        {
            RaiseFilterChanged();
        }

        private void RaiseFilterChanged()
        {
            var args = new FilterChangedEventArgs
            {
                TextSearch = txtSearch.Text,
                SelectedCategory = (cmbCategory.SelectedItem as Category)?.Name,
                SelectedSubcategory = (cmbSubcategory.SelectedItem as Subcategory)?.Name,
                MinPrice = double.TryParse(numMinPrice.Text, out var min) ? min : (double?)null,
                MaxPrice = double.TryParse(numMaxPrice.Text, out var max) ? max : (double?)null,
                SortOption = cmbSort.SelectedItem?.ToString() ?? string.Empty
            };

            // Raise the event or command
            ViewModel.ApplyFilter(args);
            if (FilterChangedCommand?.CanExecute(args) == true)
                FilterChangedCommand.Execute(args);
        }

        // DependencyProperty already in your code:
        public static readonly DependencyProperty FilterChangedCommandProperty =
            DependencyProperty.Register(
                nameof(FilterChangedCommand),
                typeof(ICommand),
                typeof(ProductFilterControl));

        public ICommand FilterChangedCommand
        {
            get => (ICommand)GetValue(FilterChangedCommandProperty);
            set => SetValue(FilterChangedCommandProperty, value);
        }

    }
    public class FilterChangedEventArgs : EventArgs
    {
        public string TextSearch { get; set; } = string.Empty;
        public string SelectedCategory { get; set; }
        public string SelectedSubcategory { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string SortOption { get; set; }
    }

}
