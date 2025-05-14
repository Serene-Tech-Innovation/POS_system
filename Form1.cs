using System.Diagnostics;
using System.Text;
using Npgsql;

namespace POS
{
    public partial class Form1 : Form
    {

        Dictionary<string, decimal> products = new Dictionary<string, decimal>();
        Dictionary<string, string> productImages = new Dictionary<string, string>();
        Dictionary<string, List<string>> categoryToSubcategories = new();
        Dictionary<string, List<string>> subcategoryToItems = new();

        private void LoadProductsFromDB()
        {
            string connString = "Host=localhost;Username=admin;Password=tooshort;Database=POS-Sys_Def";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                //TRY
                // Load products and images
                try
                {
                    //throw new NpgsqlException("Simulated error for testing");
                    using (var cmd = new NpgsqlCommand("SELECT name, price, image_path FROM item", conn))
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

                    // Load categories and subcategories
                    using (var cmd = new NpgsqlCommand("SELECT c.name AS category_name, s.name AS subcategory_name FROM subcategory s JOIN category c ON s.category_id = c.id", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string category = reader.GetString(0);
                            string subcategory = reader.GetString(1);
                            if (!categoryToSubcategories.ContainsKey(category))
                                categoryToSubcategories[category] = new List<string>();
                            categoryToSubcategories[category].Add(subcategory);
                        }
                    }

                    // Load subcategories to items
                    using (var cmd = new NpgsqlCommand("SELECT s.name AS subcategory_name, i.name AS item_name FROM item i JOIN subcategory s ON i.subcategory_id = s.id", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string subcategory = reader.GetString(0);
                            string item = reader.GetString(1);
                            if (!subcategoryToItems.ContainsKey(subcategory))
                                subcategoryToItems[subcategory] = new List<string>();
                            subcategoryToItems[subcategory].Add(item);
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    Debug.WriteLine("Error loading products: " + ex.Message);
                    products = new Dictionary<string, decimal>
                    {
                        { "C Momo", 250 },
                        { "Chicken Chilly", 300 },
                        { "Fried Chicken", 320 },
                        { "Fried Rice", 180 },
                        { "Fries", 120 },
                        { "Momo", 150 },
                        { "Pasta", 280 },
                        { "Pizza", 400 },
                        { "Salad", 160 },
                        { "Spaghetti", 300 }
                    };

                    productImages = new Dictionary<string, string>
                    {
                        { "C Momo", "c_momo.jpg" },
                        { "Chicken Chilly", "chicken_chilly.jpg" },
                        { "Fried Chicken", "fried_chicken.jpg" },
                        { "Fried Rice", "fried_rice.jpg" },
                        { "Fries", "fries.jpg" },
                        { "Momo", "momo.jpg" },
                        { "Pasta", "pasta.jpg" },
                        { "Pizza", "pizza.jpg" },
                        { "Salad", "salad.jpg" },
                        { "Spaghetti", "spaghetti.jpg" }
                    };

                }



                conn.Close();

                //
            }
        }

        // Quantity tracker
        Dictionary<string, int> cartQuantities = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
            LoadProductsFromDB();   //Load from Postgres DB
            cmbCategory.Items.AddRange(categoryToSubcategories.Keys.ToArray());
            cmbCategory.SelectedIndexChanged += (s, e) =>
            {
                cmbSubcategory.Items.Clear();
                if (cmbCategory.SelectedItem != null)
                    cmbSubcategory.Items.AddRange(categoryToSubcategories[cmbCategory.SelectedItem.ToString()].ToArray());
            };


            foreach (var product in products)
            {
                AddProductToPanel(product.Key, product.Value);
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

           
            receipt.AppendLine("        PANDEY KHAJA GHAR");
            receipt.AppendLine("        Durbar Marg, Kathmandu");
            receipt.AppendLine("        Tel: 01-12345678");
            receipt.AppendLine("****************************************************");
            receipt.AppendLine("                RECEIPT");
            receipt.AppendLine("****************************************************");
            receipt.AppendLine(string.Format("{0,-20}{1,5}{2,10}", "Item", "Qty", "Price"));

           
            foreach (var item in cartQuantities)
            {
                string productName = item.Key;
                int quantity = item.Value;

                if (products.ContainsKey(productName))
                {
                    decimal price = products[productName];
                    decimal subtotal = price * quantity;
                    total += subtotal;

                    receipt.AppendLine(string.Format("{0,-20}{1,5}{2,10}", productName, quantity, $"Rs.{subtotal}"));
                }
            }

           
            receipt.AppendLine("****************************************************");
            receipt.AppendLine(string.Format("{0,-25}{1,10}", "TOTAL:", $"Rs.{total}"));
            receipt.AppendLine(string.Format("{0,-25}{1,10}", "Discount:", "Rs.0.00"));
            receipt.AppendLine("****************************************************");
            receipt.AppendLine("         THANK YOU FOR VISITING!");
            receipt.AppendLine();
            receipt.AppendLine("        ||||||||||||||||||||| ||||||||||||||||||||"); 
            

            // Save to DB and show
            SaveOrderToDatabase(cartQuantities, total);
            MessageBox.Show(receipt.ToString(), "Checkout Summary");

           
            cartQuantities.Clear();
            UpdateCartDisplay();
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

        private void ApplyFilters()
        {
            flowPanelItems.Controls.Clear();

            string searchText = txtSearch.Text.ToLower();
            string selectedCategory = cmbCategory.SelectedItem?.ToString();
            string selectedSubcategory = cmbSubcategory.SelectedItem?.ToString();
            decimal minPrice = numMinPrice.Value;
            decimal maxPrice = numMaxPrice.Value;
            string sortOption = cmbSort.SelectedItem?.ToString();

            // Determine valid products
            var filteredProducts = products
                .Where(p =>
                    (string.IsNullOrEmpty(searchText) || p.Key.ToLower().Contains(searchText)) &&
                    (!string.IsNullOrEmpty(selectedCategory) ?
                        categoryToSubcategories[selectedCategory].SelectMany(sc => subcategoryToItems.ContainsKey(sc) ? subcategoryToItems[sc] : new List<string>()).Contains(p.Key)
                        : true) &&
                    (!string.IsNullOrEmpty(selectedSubcategory) ?
                        (subcategoryToItems.ContainsKey(selectedSubcategory) && subcategoryToItems[selectedSubcategory].Contains(p.Key))
                        : true) &&
                    (p.Value >= minPrice && p.Value <= maxPrice)
                );

            // Sorting
            filteredProducts = sortOption switch
            {
                "Price Low to High" => filteredProducts.OrderBy(p => p.Value),
                "Price High to Low" => filteredProducts.OrderByDescending(p => p.Value),
                "A-Z" => filteredProducts.OrderBy(p => p.Key),
                "Z-A" => filteredProducts.OrderByDescending(p => p.Key),
                _ => filteredProducts
            };

            // Recreate product panels
            foreach (var product in filteredProducts)
            {
                AddProductToPanel(product.Key, product.Value);
            }
        }

        private void AddProductToPanel(string name, decimal price)
        {

            flowPanelItems.SuspendLayout(); // Start layout suspension
            var productPanel = new Panel
            {
                Size = new Size(150, 200),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            var productLabel = new Label
            {
                Text = name,
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

            string imagePath = Path.Combine("../../../Images", productImages.ContainsKey(name) ? productImages[name] : name.ToLower().Replace(" ", "_") + ".jpg");
            Debug.WriteLine($"Image path for {name}: {imagePath}");

            if (File.Exists(imagePath))
                using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    productImage.Image = Image.FromStream(fs);
                }
            else
                productImage.BackColor = Color.LightGray;
            var productPrice = new Label
            {
                Text = $"Rs. {price}",
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
                Tag = name
            };
            addButton.Click += (sender, e) =>
            {
                string productName = ((Button)sender).Tag.ToString();
                Console.WriteLine(productName);
                if (cartQuantities.ContainsKey(productName))
                {

                    cartQuantities[productName]++;
                }

                else
                    cartQuantities[productName] = 1;

                UpdateCartDisplay();
            };

            productPanel.Controls.Add(addButton);
            productPanel.Controls.Add(productPrice);
            productPanel.Controls.Add(productImage);
            productPanel.Controls.Add(productLabel);

            flowPanelItems.Controls.Add(productPanel);
            flowPanelItems.ResumeLayout(); // Resume layout
        }

        private void numMinPrice_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void numMaxPrice_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbSubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }


        private void SaveOrderToDatabase(Dictionary<string, int> cartItems, decimal total)
        {
            string connString = "Host=localhost;Username=admin;Password=tooshort;Database=POS-Sys_Def";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert order
                        int orderId;
                        using (var cmd = new NpgsqlCommand("INSERT INTO orders (total_amount) VALUES (@total) RETURNING id", conn))
                        {
                            cmd.Parameters.AddWithValue("@total", total);
                            orderId = (int)cmd.ExecuteScalar();
                        }

                        // Insert order items
                        foreach (var item in cartItems)
                        {
                            string name = item.Key;
                            int qty = item.Value;
                            decimal price = products[name];

                            using (var cmd = new NpgsqlCommand("INSERT INTO order_items (order_id, item_name, quantity, unit_price) VALUES (@order_id, @name, @qty, @price)", conn))
                            {
                                cmd.Parameters.AddWithValue("@order_id", orderId);
                                cmd.Parameters.AddWithValue("@name", name);
                                cmd.Parameters.AddWithValue("@qty", qty);
                                cmd.Parameters.AddWithValue("@price", price);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Order save failed: " + ex.Message);
                        transaction.Rollback();
                        MessageBox.Show("Failed to save the order. Please try again.");
                    }
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

        }
    }

}
