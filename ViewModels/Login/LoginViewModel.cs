using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Npgsql;
using POS.Helpers.Relays;

namespace POS.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = "admin_test";
        private string _password = "hashed_password";
        private string _errorMessage;
        private Visibility _errorVisibility = Visibility.Collapsed;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public Visibility ErrorVisibility
        {
            get => _errorVisibility;
            set { _errorVisibility = value; OnPropertyChanged(); }
        }
        public ICommand LoginCommand { get; }

        private readonly Action<Page> _navigateAction;

        public LoginViewModel(Action<Page> navigateAction)
        {
            _navigateAction = navigateAction;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object? parameter)
        {
            if (ValidateCredentials(Username, Password))
            {
                string role = GetUserRole(Username);
                // Navigate or update state as needed
                MessageBox.Show($"Login successful! Role: {role}", "Success", MessageBoxButton.OK);
                NavigatedHandler(role);
            }
            else
            {
                ErrorMessage = "Invalid credentials. Please try again.";
                ErrorVisibility = Visibility.Visible;
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Connection string not found in App.config", "Error");
                return false;
            }

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password_hash = @password";
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error");
                return false;
            }
        }

        private string GetUserRole(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                return "user";

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                string query = "SELECT role FROM users WHERE username = @username";
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("username", username);
                var result = command.ExecuteScalar();
                return result?.ToString() ?? "user";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Role fetch error: {ex.Message}", "Error");
                return "user";
            }
        }

        private void NavigatedHandler(string role)
        {
            switch (role)
            {
                case "user":
                case "moderator":
                case "admin":
                    _navigateAction(new DashboardPage(role));
                    break;
                case "server":
                case "cashier":
                    _navigateAction(new OrderPage(_navigateAction, role));
                    break;
                default:
                    MessageBox.Show("Invalid role", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _navigateAction(new DashboardPage());
                    break;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}