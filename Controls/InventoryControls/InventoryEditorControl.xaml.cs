using POS.Models.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for InventoryEditorControl.xaml
    /// </summary>
    public partial class InventoryEditorControl : UserControl
    {

        public ObservableCollection<EditableProduct> _editableProducts;
        public Product _product;
        public List<Category> CategoryList => ProductDataStore.Categories;

        public void AddEditableProduct(EditableProduct editableProduct, Product product)
        {
            if (!_editableProducts.Any(p => p.Name == editableProduct.Name))
                _editableProducts.Add(editableProduct);
            _product = product;
            name.SetValue(TextBox.TextProperty, _product.Name);
        }

        public InventoryEditorControl()
        {
            InitializeComponent();

            // Initialize the editable products collection
            //DataContext = ProductDataStore.Products;
            _editableProducts = new ObservableCollection<EditableProduct>();
            _product = new Product();
            Loaded += InventoryEditorControl_Loaded;
            
            ProductGrid.ItemsSource = _editableProducts;

            //ProductGrid.DataContext = _editableProducts;
        }

        private void InventoryEditorControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ProductDataStore.GetProduct(_editableProducts.ToString());
            
        }

        private void ApplySingle_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is EditableProduct ep)
            {
                ProductDataStore.SetProduct(ep.ToProduct());
                MessageBox.Show($"Updated: {ep.Name}");
            }
        }

        private void ApplyAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var ep in _editableProducts)
                ProductDataStore.SetProduct(ep.ToProduct());

            MessageBox.Show("All changes applied.");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is EditableProduct ep)
            {
                var original = ProductDataStore.GetProduct(ep.Name);
                if (original != null)
                    ProductDataStore.Products.Remove(original);

                _editableProducts.Remove(ep);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class EditableProduct
    {
        public EditableProduct(Product product)
        {
            Name = product.Name;
            Price = product.Price;
            Stock = product.Stock;
            Category = product.Category;
            Subcategory = product.Subcategory;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; } = 0;
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }

        public Product ToProduct()
        {
            return new Product
            {
                Name = this.Name,
                Price = this.Price,
                Stock = this.Stock,
                Category = this.Category,
                Subcategory = this.Subcategory
            };
        }

    }
}
