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
    public partial class Recipt : Window
    {
       
        public Recipt()
        {
            InitializeComponent();
        }

        public Recipt(Dictionary<string, int> cartQuantities, Dictionary<string, decimal> products)
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

        //private void PrintReceiptPdf()
        //{
        //    try
        //    {
        //        PrintDialog printDialog = new PrintDialog();
        //        if (printDialog.ShowDialog() == true)
        //        {

        //            double originalWidth = ReceiptPanel.Width;
        //            double originalHeight = ReceiptPanel.Height;


        //            ReceiptPanel.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
        //            ReceiptPanel.Arrange(new Rect(new Point(0, 0), ReceiptPanel.DesiredSize));

        //            printDialog.PrintVisual(ReceiptPanel, "Receipt");



        //            ReceiptPanel.Measure(new Size(originalWidth, originalHeight));
        //            ReceiptPanel.Arrange(new Rect(new Size(originalWidth, originalHeight)));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occoured while printing: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}


        private void PrintReceipt()
        {
            try
            {
                // Get list of installed printers (via PrintServer)
                var printServer = new System.Printing.LocalPrintServer();
                var queues = printServer.GetPrintQueues(new[] {
            System.Printing.EnumeratedPrintQueueTypes.Local,
            System.Printing.EnumeratedPrintQueueTypes.Connections
        });

                
                bool hasPrinter = queues.Any(q =>
                {
                    try
                    {
                        q.Refresh(); 
                        return !q.IsOffline && q.IsShared && q.QueueStatus == System.Printing.PrintQueueStatus.None;
                    }
                    catch
                    {
                        return false; 
                    }
                });

                if (!hasPrinter)
                {
                    //MessageBox.Show("No available printer found or all printers are offline.",
                    //               "Printer Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);

                    var dialog = new PrinterErrorWindow();
                    bool? result = dialog.ShowDialog();

                    if (result != true)
                    {
                        // User clicked "OK" or closed the dialog, do not print
                        return;
                    }
                    
                    
                }

               
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    double originalWidth = ReceiptPanel.Width;
                    double originalHeight = ReceiptPanel.Height;

                    ReceiptPanel.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                    ReceiptPanel.Arrange(new Rect(new Point(0, 0), ReceiptPanel.DesiredSize));

                    printDialog.PrintVisual(ReceiptPanel, "Receipt");

                    ReceiptPanel.Measure(new Size(originalWidth, originalHeight));
                    ReceiptPanel.Arrange(new Rect(new Size(originalWidth, originalHeight)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while printing:\n" + ex.Message,
                                "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public class ReceiptItem
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintReceipt(); 
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
