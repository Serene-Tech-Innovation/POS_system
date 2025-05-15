using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        // Method to validate login credentials with the database
        public bool ValidateCredentials(string username, string password)
        {
            bool isValid = false;
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Connection string not found in App.config", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to check if username and password match
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password_hash = @password";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        isValid = count > 0;  // If count is greater than 0, credentials are valid
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isValid;
        }

        // Updated code to fix CS8602 and CS8600 errors
        public string GetUserRole(string username)
        {
            string role = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Connection string not found in App.config", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "user";
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to retrieve the user's role based on username
                    string query = "SELECT role FROM users WHERE username = @username";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("username", username);
                        var result = command.ExecuteScalar();
                        role = result?.ToString() ?? "user"; // Safely handle null values
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return role;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}