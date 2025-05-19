using POS.Models.Core;
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
using System.Windows.Shapes;

namespace POS
{
    /// <summary>
    /// Interaction logic for QuickProductEditWindow.xaml
    /// </summary>
    public partial class QuickProductEditWindow : Window
    {

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

        public QuickProductEditWindow(Product product)
        {
            InitializeComponent();
            LoadProductToWindow(product);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
