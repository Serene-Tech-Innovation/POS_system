using System;
using System.Windows;
using Npgsql;  // Required for PostgreSQL interaction
using System.Configuration;

namespace temp.POS
{
    public partial class Login : Window
    {
        public Login(String t)
        {
            InitializeComponent();
        }

        // Event handler for login button click
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Developement Case
            var cashier = new NewOrder("cashier");
            cashier.Show();
            this.Hide(); // Hide the current form
            return; // For development purposes, show the cashier form directly
            //Basic Login Successful
            // Get user input
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            // Validate credentials with database
            if (ValidateCredentials(username, password))
            {
                // Retrieve the role of the user
                string role = GetUserRole(username);
                
                // Open corresponding form based on role
                if (role == "user")
                {
                    var userForm = new escalatePerms(); // For regular user, show user dashboard form
                    userForm.Show();
                }
                else
                {
                    // Handle other roles if needed
                    MessageBox.Show($"Welcome, {role}!", "Role-based Access", MessageBoxButton.OK, MessageBoxImage.Information);
                    var newOrderForm = new NewOrder(role); // For admin, show new order form
                    newOrderForm.Show();
                }

                this.Close(); // Close the Login form after transition
            }
            else
            {
                MessageBox.Show("Invalid credentials", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to validate login credentials with the database
        private bool ValidateCredentials(string username, string password)
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
        private string GetUserRole(string username)
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
