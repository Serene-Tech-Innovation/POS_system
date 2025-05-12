using System.Text;
using Npgsql;

namespace POS
{
    public partial class Form1 : Form
    {
        //Dict to soft load.
        //TODO: Add additional fields to search and sort functions.
        //Dictionary<string, decimal> products = new Dictionary<string, decimal>
        //{
        //    { "Momo", 200m },
        //    { "Pizza", 700m },
        //    { "Chicken Chilly", 200m },
        //    { "C.momo", 240m },
        //    { "Salad", 100m },
        //    { "Fried Rice", 300m },
        //    { "Spaghetti", 250m },
        //    { "Fried Chicken", 1420m },
        //    { "Pasta", 210m },
        //    { "Fries", 180m }
        //};

        Dictionary<string, decimal> products = new Dictionary<string, decimal>();
        Dictionary<string, string> productImages = new Dictionary<string, string>();

        private void LoadProductsFromDB()
        {
            string connString = "Host=localhost;Username=admin;Password=tooshort;Database=POS-Sys_Def";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT name, price, image_path FROM products", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        decimal price = reader.GetDecimal(1);
                        string imagePath = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        products[name] = price;
                        productImages[name] = imagePath;
                    }
                }
            }

            if (products == null)
            {
                products = new Dictionary<string, decimal>
                {
                    { "Momo", 200m },
                    { "Pizza", 700m },
                    { "Chicken Chilly", 200m },
                    { "C.momo", 240m },
                    { "Salad", 100m },
                    { "Fried Rice", 300m },
                    { "Spaghetti", 250m },
                    { "Fried Chicken", 1420m },
                    { "Pasta", 210m },
                    { "Fries", 180m }
                };
            }
        }

        // Quantity tracker
        Dictionary<string, int> cartQuantities = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
            LoadProductsFromDB();   //Load from Postgres DB
            foreach (var product in products)
            {
                Console.WriteLine(product);
                var productPanel = new Panel
                {
                    Size = new Size(150, 200),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10)
                };

                var productLabel = new Label
                {
                    Text = product.Key,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Height = 40
                };

                var productImage = new PictureBox
                {
                    Dock = DockStyle.Top,
                    Height = 80,
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                //string imagePath = Path.Combine("../../../Images", product.Key.ToLower().Replace(" ", "_") + ".jpg"); //For Dict Only Load
                string imagePath = Path.Combine("../../../", productImages[product.Key]); //For Postgres DB Load


                if (File.Exists(imagePath))
                {
                    productImage.Image = Image.FromFile(imagePath);
                }
                else
                {
                    imagePath = Path.Combine("../../../Images", product.Key.ToLower().Replace(" ", "_") + ".jpg"); //For Dict Only Load
                    if (File.Exists(imagePath))
                    {
                        productImage.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        productImage.BackColor = Color.LightGray;
                        productImage.Image = null;
                    }
                }

                var productPrice = new Label
                {
                    Text = $"Rs. {product.Value}",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 18, FontStyle.Bold),
                    Padding = new Padding(0, 5, 0, 0),
                    Height = 30
                };

                var addButton = new Button
                {
                    Text = "Add to Cart",
                    Height = 30,
                    Dock = DockStyle.Bottom,
                    Tag = product.Key
                };

                addButton.Click += (sender, e) =>
                {
                    string productName = ((Button)sender).Tag.ToString();

                    if (cartQuantities.ContainsKey(productName))
                    {
                        cartQuantities[productName]++;
                    }
                    else
                    {
                        cartQuantities[productName] = 1;
                    }

                    UpdateCartDisplay();
                };

                productPanel.Controls.Add(addButton);
                productPanel.Controls.Add(productPrice);
                productPanel.Controls.Add(productImage);
                productPanel.Controls.Add(productLabel);

                flowPanelItems.Controls.Add(productPanel);
            }
        }

        private void UpdateCartDisplay()
        {
            cartListBox.Items.Clear();
            cartListBox.Items.Add(string.Format("{0,-15} {1,16} {2,25}", "Item", "Qty", "Price"));

            foreach (var item in cartQuantities)
            {
                string name = item.Key;
                int quantity = item.Value;
                decimal unitPrice = products[name];
                decimal subtotal = unitPrice * quantity;

                string line = string.Format("{0,-20} {1,10} Rs.{2,10}", name, quantity, subtotal);
                cartListBox.Items.Add(line);
            }

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = 0;

            foreach (var item in cartQuantities)
            {
                if (products.ContainsKey(item.Key))
                {
                    total += products[item.Key] * item.Value;
                }
            }

            lblTotal.Text = $"Total: Rs. {total}";
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (cartQuantities.Count == 0)
            {
                MessageBox.Show("Your cart is empty.");
                return;
            }

            StringBuilder receipt = new StringBuilder();
            decimal total = 0;

            receipt.AppendLine("Items in Cart:\n");

            foreach (var item in cartQuantities)
            {
                string productName = item.Key;
                int quantity = item.Value;

                if (products.ContainsKey(productName))
                {
                    decimal price = products[productName];
                    decimal subtotal = price * quantity;
                    total += subtotal;
                    receipt.AppendLine($"{productName} x{quantity} - Rs. {subtotal}");
                }
            }

            receipt.AppendLine($"\nTotal: Rs. {total}");

            MessageBox.Show(receipt.ToString(), "Checkout Summary");

            // Clear the cart
            cartQuantities.Clear();
            UpdateCartDisplay();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //LoadProductsFromDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Total_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void btnCheckout_Click_1(object sender, EventArgs e)
        {
            btnCheckout_Click(sender, e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void clear_Click(object sender, EventArgs e)
        {
            cartListBox.Items.Clear();
            lblTotal.Text = "Total: Rs. 0";
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (cartListBox.SelectedIndex > 0)
            {
                string selected = cartListBox.SelectedItem.ToString();


                string itemName = selected.Substring(0, 20).Trim();

                if (cartQuantities.ContainsKey(itemName))
                {
                    if (cartQuantities[itemName] > 1)
                        cartQuantities[itemName]--;
                    else
                        cartQuantities.Remove(itemName);

                    UpdateCartDisplay();
                }
            }
        }
    }
}
