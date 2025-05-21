using POS.Models.Core;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace POS
{
    /// <summary>
    /// Interaction logic for QuickProductEditWindow.xaml
    /// </summary>
    public partial class QuickProductEditWindow : Window
    {
        private bool _isEdit = false;

        private void LoadProductToWindow(Product product)
        {
            if (product == null) return;
            NameBox.Text = product.Name;
            PriceBox.Text = product.Price.ToString();
            StockBox.Text = product.Stock.ToString();
            if (product.Category != null)
            {
                foreach (var category in ProductDataStore.Categories)
                {
                    CategoryBox.Items.Add(category.Name);
                }
                foreach (var subcategory in product.Category.Subcategories)
                {
                    SubCategoryBox.Items.Add(subcategory.Name);
                }
                //CategoryBox.ItemsSource = ProductDataStore.Categories;
                CategoryBox.SelectedItem = product.Category.Name;
                //SubCategoryBox.ItemsSource = product.Category.Subcategories;
                SubCategoryBox.SelectedIndex = SubCategoryBox.Items.IndexOf(product.Subcategory.Name);
            }
        }

        public QuickProductEditWindow()
        {
            InitializeComponent();
            //LoadProductToWindow(product);
        }

        public QuickProductEditWindow(Product product)
        {
            InitializeComponent();
            _isEdit = true;
            LoadProductToWindow(product);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_isEdit)
            {
            }
            else
            {
                string name = NameBox.Text;
                decimal price;
                int stock;
                // Try to parse the price and stock input to an integer  
                if (!decimal.TryParse(PriceBox.Text, out price))
                {
                    MessageBox.Show("Please enter a valid numeric value for the price.");
                    return;
                }
                if (!int.TryParse(StockBox.Text, out stock))
                {
                    MessageBox.Show("Please enter a valid numeric value for the stock.");
                    return;
                }

                string category, subcategory;
                category = CategoryBox.SelectedItem != null
                    ? CategoryBox.SelectedItem.ToString()
                    : CategoryBox.Text;

                subcategory = SubCategoryBox.SelectedItem != null
                    ? SubCategoryBox.SelectedItem.ToString()
                    : SubCategoryBox.Text;
                if(ProductDataStore.GetCategory(category)==null)
                    ProductDataStore.SetCategory(new Category { Name = category });
                if (ProductDataStore.GetSubcategory(category, subcategory) == null)
                    ProductDataStore.SetSubcategory(category ,new Subcategory { Name = subcategory });

                var product = new Product
                {
                    Name = name,
                    Price = price,
                    Stock = stock,
                    Category = ProductDataStore.GetCategory(category),
                    Subcategory = ProductDataStore.GetSubcategory(category, subcategory)
                };
                ProductDataStore.SetProduct(product);
                var pro = ProductDataStore.GetProduct(name);
                if(pro != null)
                {
                    Debug.WriteLine(pro.Name);
                    Debug.WriteLine(pro.Price);
                    Debug.WriteLine(pro.Stock);
                    Debug.WriteLine(pro.Category.Name);
                    Debug.WriteLine(pro.Subcategory.Name);
                }
            }
            Debug.WriteLine("Product saved successfully.");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
