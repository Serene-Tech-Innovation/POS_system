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
                    throw new NpgsqlException("Simulated error for testing");
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
                        reader.Close();
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
                        reader.Close();
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
                        reader.Close();
                    }
                }
                catch(NpgsqlException ex)
                {
                    Console.WriteLine("Error loading products: " + ex.Message);
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

            btnFilter.Click += (s, e) => ApplyFilters();
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            cmbSort.SelectedIndexChanged += (s, e) => ApplyFilters();

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

                Debug.WriteLine(productImages[product.Key]);
                //string imagePath = Path.Combine("../../../Images", product.Key.ToLower().Replace(" ", "_") + ".jpg"); //For Dict Only Load
                string imagePath = Path.Combine("../../../Images", productImages[product.Key]); //For Postgres DB Load


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
            if (File.Exists(imagePath))
                productImage.Image = Image.FromFile(imagePath);
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
                if (cartQuantities.ContainsKey(productName))
                    cartQuantities[productName]++;
                else
                    cartQuantities[productName] = 1;

                UpdateCartDisplay();
            };

            productPanel.Controls.Add(addButton);
            productPanel.Controls.Add(productPrice);
            productPanel.Controls.Add(productImage);
            productPanel.Controls.Add(productLabel);

            flowPanelItems.Controls.Add(productPanel);
        }
    


    }
}
