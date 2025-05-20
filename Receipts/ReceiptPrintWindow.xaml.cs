using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private ReceiptPreviewPage _receiptPreviewPage;

        public void PrintReceipt()
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

                    var dialog = new ConfirmationDialog("No available printer found or all printers are offline.");
                    dialog.yesBtn.Content = "Print to PDF";
                    dialog.noBtn.Content = "Cancel";
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
                    // Get the original size of the ReceiptPanel
                    double originalWidth = _receiptPreviewPage.ActualWidth;
                    double originalHeight = _receiptPreviewPage.ActualHeight;

                    Debug.WriteLine($"Original Width: {originalWidth}, Original Height: {originalHeight}");

                    _receiptPreviewPage.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                    _receiptPreviewPage.Arrange(new Rect(new Point(0, 0), _receiptPreviewPage.DesiredSize));

                    printDialog.PrintVisual(_receiptPreviewPage, "Receipt");

                    _receiptPreviewPage.Measure(new Size(originalWidth, originalHeight));
                    _receiptPreviewPage.Arrange(new Rect(new Size(originalWidth, originalHeight)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while printing:\n" + ex.Message,
                                "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ReceiptPrintWindow(Dictionary<string, int> cartQuantities, Dictionary<string, decimal> products)
        {
            InitializeComponent();
            _cartQuantities = cartQuantities;
            _products = products;

            // Pass data to embedded ReceiptPreviewPage
            _receiptPreviewPage = new ReceiptPreviewPage(_cartQuantities, _products);
            receiptPreviewPage.Content = _receiptPreviewPage;
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            //Code to print receipt
            PrintReceipt();
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
