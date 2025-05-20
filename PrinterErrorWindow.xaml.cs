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
    /// Interaction logic for PrinterErrorWindow.xaml
    /// </summary>
    public partial class PrinterErrorWindow : Window
    {
        public bool ShouldPrintToPDF { get; set; } = false; // Property to control PDF printing
        public PrinterErrorWindow()
        {
            InitializeComponent();
        }
        

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ShouldPrintToPDF = false;
            this.DialogResult = false;
            this.Close();
        }

        private void PrintPdfButton_Click(object sender, RoutedEventArgs e)
        {
            ShouldPrintToPDF = true;
            this.DialogResult = true;   
            this.Close();   
        }
    }
}
