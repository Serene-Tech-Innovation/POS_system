using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for ReceiptPreviewPage.xaml
    /// </summary>
    public partial class ReceiptPreviewPage : Page
    {
        private readonly Dictionary<string, int> _cart;
        private readonly Dictionary<string, decimal> _products;

        public class ReceiptItem
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total => Quantity * Price;
        }


        private void LoadToReceipt(Dictionary<string, int> cart, Dictionary<string, decimal> products)
        {
            var receiptItems = new List<ReceiptItem>();
            decimal subtotal = 0;

            foreach (var item in cart)
            {
                if (products.TryGetValue(item.Key, out decimal price))
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

            // Find textblocks inside the receipt to update values
            UpdateTextBlockValue(subTotalValue, subtotal);
            UpdateTextBlockValue(taxValue, tax);
            UpdateTextBlockValue(totalValue, total);
        }

        private void UpdateTextBlockValue(TextBlock textBlock, decimal value)
        {
            textBlock.Text = value.ToString();
        }


        public ReceiptPreviewPage()
        {
            InitializeComponent();
        }
        public ReceiptPreviewPage(Dictionary<string, int> cart, Dictionary<string, decimal> products)
        {
            InitializeComponent();
            _cart = cart;
            _products = products;
            LoadToReceipt(_cart, _products);
        }


        private void Print_Click(object sender, RoutedEventArgs e)
        {
            ReceiptPrintWindow receiptPrintWindow = new ReceiptPrintWindow(_cart, _products);
            receiptPrintWindow.ShowDialog();
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
