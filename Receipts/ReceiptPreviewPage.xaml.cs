using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using POS.Models.Core; // Assuming CartItem and Products models exist here
using POS.Models.Transaction;

namespace POS
{
    /// <summary>
    /// Interaction logic for ReceiptPreviewPage.xaml
    /// </summary>
    public partial class ReceiptPreviewPage : Page
    {
        private readonly Dictionary<string, int> _cartQuantities;
        private readonly Dictionary<string, decimal> _products;

        public ReceiptPreviewPage()
        {
            InitializeComponent();
            _cartQuantities = new Dictionary<string, int>();
            _products = new Dictionary<string, decimal>();
        }

        public ReceiptPreviewPage(Dictionary<string, int> cartQuantities, Dictionary<string, decimal> products)
        {
            InitializeComponent();
            _cartQuantities = cartQuantities;
            _products = products;
            LoadToReceipt();
        }

        private void LoadToReceipt()
        {
            var receiptItems = new List<ReceiptItem>();
            decimal subtotal = 0;

            foreach (var item in _cartQuantities)
            {
                if (_products.TryGetValue(item.Key, out decimal price))
                {
                    var receiptItem = new ReceiptItem
                    {
                        Name = item.Key,
                        Quantity = item.Value,
                        Price = price
                    };
                    subtotal += receiptItem.Total;
                    receiptItems.Add(receiptItem);
                }
            }

            ReceiptItems.ItemsSource = receiptItems;

            decimal tax = Math.Round(subtotal * 0.13m, 2); // 13% tax
            decimal total = subtotal + tax;

            UpdateTextBlockValue(subTotalValue, subtotal);
            UpdateTextBlockValue(taxValue, tax);
            UpdateTextBlockValue(totalValue, total);
        }

        private void UpdateTextBlockValue(TextBlock textBlock, decimal value)
        {
            textBlock.Text = $"Rs. {value:F2}";
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            //Code to print receipt
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for emailing receipt logic
            MessageBox.Show("Feature not implemented yet.");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // NavigationService?.GoBack(); // If navigating
            Window.GetWindow(this)?.Close(); // If in standalone window
        }
    }
}
