using Npgsql;
using POS.Models.Core;
using System.Configuration;
using System.Runtime.CompilerServices;
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
        private void LoadProductsFromDB()
        {
            string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            ProductDataStore.ClearAll();

            var categories = new Dictionary<int, Category>();
            var subcategories = new Dictionary<int, Subcategory>();

            // Load Categories
            using (var cmd = new NpgsqlCommand("SELECT id, name FROM category", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    var category = new Category { Name = name };
                    categories[id] = category;
                    ProductDataStore.SetCategory(category);
                }
            }

            // Load Subcategories
            using (var cmd = new NpgsqlCommand("SELECT id, name, category_id FROM subcategory", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    int categoryId = reader.GetInt32(2);

                    if (!categories.ContainsKey(categoryId))
                        continue;

                    var subcategory = new Subcategory
                    {
                        Name = name,
                        ParentCategory = categories[categoryId]
                    };

                    subcategories[id] = subcategory;
                    ProductDataStore.SetSubcategory(categories[categoryId].Name, subcategory);
                }
            }

            // Load Products
            using (var cmd = new NpgsqlCommand("SELECT name, price, subcategory_id, image_path FROM item", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    decimal price = reader.GetDecimal(1);
                    int subcategoryId = reader.GetInt32(2);
                    string imagePath = reader.IsDBNull(3) ? "" : reader.GetString(3);

                    if (!subcategories.ContainsKey(subcategoryId))
                        continue;

                    var subcategory = subcategories[subcategoryId];
                    var product = new Product
                    {
                        Name = name,
                        Price = price,
                        Category = subcategory.ParentCategory,
                        Subcategory = subcategory
                    };

                    ProductDataStore.SetProduct(product);
                }
            }
        }

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
                    LoadProductsFromDB();
                    String role = loginPage.GetUserRole(username);
                    if (role == "user")
                    {
                        MainFrame.Navigate(new DashboardPage(role));
                    }
                    else if (role == "server")
                    {
                        MainFrame.Navigate(new OrderPage(this, role));
                    }
                    else if (role == "cashier")
                    {
                        MainFrame.Navigate(new OrderPage(this, role));
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

        internal static MainWindow GetWindow()
        {
            return Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }
    }
}