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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS
{
    /// <summary>
    /// Interaction logic for CustomerManagementPage.xaml
    /// </summary>
    public partial class CustomerManagementPage : Page
    {
        public CustomerManagementPage()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            // In a real app, load from database
            //CustomersGrid.ItemsSource = new List<Customer>
            //    {
            //        new Customer { Id = 1, Name = "John Doe", Phone = "555-0101", Email = "john@example.com" }
            //    };
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            // Open customer edit dialog
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            //if (CustomersGrid.SelectedItem is Customer customer)
            //{
            //    // Open edit dialog
            //}
        }
    }
}
