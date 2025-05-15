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
    public partial class Receipt : Window
    {
       
        public Receipt()
        {
            InitializeComponent();
        }

        public Receipt(Dictionary<string, int> cartQuantities, Dictionary<string, decimal> products)
        {
            InitializeComponent();
            LoadReceipt(cartQuantities, products);
        }

        private void LoadReceipt(Dictionary<string, int> cart, Dictionary<string, decimal> products)
        {
            decimal total = 0;
            var receiptItems = new List<ReceiptItem>();

            foreach (var item in cart)
            {
                string name = item.Key;
                int qty = item.Value;
                decimal price = products[name];
                decimal subtotal = price * qty;
                total += subtotal;

                receiptItems.Add(new ReceiptItem
                {
                    Name = $"{name} x{qty}",
                    Price = $"Rs. {subtotal:F2}"
                });
            }

            itemsControl.ItemsSource = receiptItems;

            totalTextBlock.Text = $"Rs. {total:F2}";
            Discountamt.Text = "Rs. 0.00"; 
        }

        
        public class ReceiptItem
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
