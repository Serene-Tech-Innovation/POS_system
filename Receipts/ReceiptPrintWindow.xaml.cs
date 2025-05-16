using System;
using System.Collections.Generic;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace POS
{
    /// <summary>
    /// Interaction logic for ReceiptPrintWindow.xaml
    /// </summary>
    public partial class ReceiptPrintWindow : Window
    {
        private readonly Dictionary<string, int> _cartQuantities;
        private readonly Dictionary<string, decimal> _products;

        public ReceiptPrintWindow(Dictionary<string, int> cartQuantities, Dictionary<string, decimal> products)
        {
            InitializeComponent();
            _cartQuantities = cartQuantities;
            _products = products;

            // Pass data to embedded ReceiptPreviewPage
            receiptPreviewPage.Content = new ReceiptPreviewPage(_cartQuantities, _products);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (receiptPreviewPage.Content is Visual visual)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(visual, "Receipt Print");
                }
            }
        }
    }
}
