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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public string _role = String.Empty;
        public DashboardPage()
        {
            InitializeComponent();
        }

        public DashboardPage(string role)
        {
            InitializeComponent();
            _role = role;
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new OrderPage(MainWindow.GetWindow(), _role));
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InventoryManagementPage());
        }

        private void Customers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CustomerManagementPage());
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportsPage());
        }
    }
}
