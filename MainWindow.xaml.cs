using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace POS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Login loginPage = new Login();
            //DashboardPage dashboardPage = new DashboardPage();
            // Navigate to login page
            MainFrame.Navigate(loginPage);
            loginPage.loginButton.Click += (s, e) =>
            {
                // Navigate to the new order page after login
                String username = loginPage.UsernameBox.Text;
                String password = loginPage.PasswordBox.Password;
                if(loginPage.ValidateCredentials(username, password))
                {
                    String role = loginPage.GetUserRole(username);
                    if (role == "user")
                    {
                        MainFrame.Navigate(new DashboardPage(role));
                    }
                    else if (role == "server")
                    {
                        MainFrame.Navigate(new OrderPage(role));
                    }
                    else if (role == "cashier")
                    {
                        MainFrame.Navigate(new OrderPage(role));
                    }
                    else if (role == "moderator")
                    {
                        MainFrame.Navigate(new DashboardPage(role));
                    }
                    else if (role == "admin")
                    {
                        MainFrame.Navigate(new DashboardPage(role));
                    }
                    else
                    {
                        MessageBox.Show("Invalid role", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        MainFrame.Navigate(new DashboardPage());
                    }
                }
            };
        }

    }
}