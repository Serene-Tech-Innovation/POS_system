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

namespace POS.Pages.Inventory
{
    /// <summary>
    /// Interaction logic for AddProducts.xaml
    /// </summary>
    public partial class AddProducts : Window
    {
        public AddProducts()
        {
            InitializeComponent();
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (dlg.ShowDialog() == true)
            {
                productImage.Source = new BitmapImage(new Uri(dlg.FileName));
                // Optionally store the file path for saving to DB later
            }
        }

        private void productNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void productPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = productNameTextBox.Text;
            int price;

            // Try to parse the price input to an integer  
            if (!int.TryParse(productPriceTextBox.Text, out price))
            {
                MessageBox.Show("Please enter a valid numeric value for the price.");
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Proceed with further logic for submitting the product  
            MessageBox.Show($"Product '{name}' with price {price} has been submitted.");
        }
    }
}
