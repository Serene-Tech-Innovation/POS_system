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
    /// Interaction logic for ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Window
    {
        string r;
        private bool _logoutClicked = false;

        public ViewOrder(string role)
        {
            InitializeComponent();
            r = role;
        }

        private void NavigateBackToLogin()
        {
            var loginForm = new Login();
            loginForm.Show();
            _logoutClicked = true; // Set the flag to indicate logout was clicked
            this.Close(); // Close the ViewOrder form
        }

        private void ReturnToNewOrder()
        {
            var newOrderForm = new NewOrder(r);
            newOrderForm.Show();
            this.Close(); // Close the ViewOrder form
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the new order form
            ReturnToNewOrder();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            // Navigate back to the new order form
            if(!_logoutClicked)
                ReturnToNewOrder();
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the login form
            NavigateBackToLogin();
        }
    }
}
