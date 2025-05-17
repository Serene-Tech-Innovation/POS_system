using POS.Models.Core;
using POS.ViewModels;
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
using POS.ViewModels;

namespace POS
{
    /// <summary>
    /// Interaction logic for InventoryManagementPage.xaml
    /// </summary>
    public partial class InventoryManagementPage : Page
    {
        public InventoryManagementViewModel ViewModel { get; set; }

        private void ProductFilterControl_FilterChanged(object? sender, FilterChangedEventArgs e)
        {
            //ProductDisplay.ApplyFilter(e);
        }

        private void OnProductClick(Product product)
        {
            // Convert Products to EditableProduct before adding to _editableProducts
            var editableProduct = new EditableProduct(product)
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Subcategory = product.Subcategory
            };

            InventoryEditor.AddEditableProduct(editableProduct);
        }



        public InventoryManagementPage()
        {
            InitializeComponent();

            //ProductFilter.RefreshCategories();
            //ProductFilter.FilterChanged += ProductFilterControl_FilterChanged;

            //ProductDisplay.ProductAddedToCart += OnProductClick;
        }
    }
}
