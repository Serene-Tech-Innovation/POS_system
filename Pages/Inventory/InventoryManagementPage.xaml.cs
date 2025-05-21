using POS.Models.Core;
using POS.Pages.Inventory;
using System;
using System.Collections.Generic;
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

namespace POS
{
    /// <summary>
    /// Interaction logic for InventoryManagementPage.xaml
    /// </summary>
    public partial class InventoryManagementPage : Page
    {
        private void ProductFilterControl_FilterChanged(object? sender, FilterChangedEventArgs e)
        {
            ProductDisplay.ApplyFilter(e);
        }

        private void OnProductClick(Product product)
        {
            // Convert Product to EditableProduct before adding to _editableProducts
            var editableProduct = new EditableProduct(product)
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Subcategory = product.Subcategory
            };

            QuickProductEditWindow quickProductEditWindow = new QuickProductEditWindow(editableProduct.ToProduct());
            quickProductEditWindow.Show();
            InventoryEditor.AddEditableProduct(editableProduct, editableProduct.ToProduct());
        }



        public InventoryManagementPage()
        {
            InitializeComponent();

            ProductFilter.RefreshCategories();
            ProductFilter.FilterChanged += ProductFilterControl_FilterChanged;

            ProductDisplay.ProductAddedToCart += OnProductClick;
        }

        private void InventoryEditor_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProducts addProductWindow = new AddProducts();
            addProductWindow.ShowDialog();
        }

        private void InventoryEditor_Loaded_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
