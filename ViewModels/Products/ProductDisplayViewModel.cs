using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using POS.Models.Core;

namespace POS.ViewModels.Products
{
    public class ProductDisplayViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Raw lists
        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<Subcategory> AllSubcategories { get; } = new();
        public ObservableCollection<Product> Products { get; } = new();

        //Grouped lists for UI
        public ObservableCollection<CategoryGroup> GroupedProducts { get; } = new();

        public ProductDisplayViewModel()
        {
            LoadProductsFromDB();
            UpdateGroupedProducts(Products);
        }

        private void LoadProductsFromDB()
        {
            ProductDataStore.ClearAll();
            Categories.Clear();
            AllSubcategories.Clear();
            Products.Clear();

            // Load categories and subcategories from the database
            string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            ProductDataStore.ClearAll();
            Products.Clear();

            var categories = new Dictionary<int, Category>();
            var subcategories = new Dictionary<int, Subcategory>();

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

            using (var cmd = new NpgsqlCommand("SELECT name, price, subcategory_id, image_path FROM item", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    decimal price = reader.GetDecimal(1);
                    int subcategoryId = reader.GetInt32(2);
                    string imagePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "Assets",
                        "Images",
                        reader.IsDBNull(3) ? "" : reader.GetString(3)
                    );

                    if (!subcategories.ContainsKey(subcategoryId))
                        continue;

                    var subcategory = subcategories[subcategoryId];
                    var product = new Product
                    {
                        Name = name,
                        Price = price,
                        Category = subcategory.ParentCategory,
                        Subcategory = subcategory,
                        ImagePath = imagePath
                    };

                    ProductDataStore.SetProduct(product);
                    Products.Add(product);
                }
            }

            //OnPropertyChanged(nameof(Products));
        }

        public void UpdateGroupedProducts(IEnumerable<Product> filtered)
        {
            GroupedProducts.Clear();

            if (filtered == null || !filtered.Any())
                return;
            var grouped = filtered
                .GroupBy(p => p.Category.Name)
                .Select(catGroup => new CategoryGroup
                {
                    CategoryName = catGroup.Key,
                    Subcategories = catGroup
                        .GroupBy(p => p.Subcategory.Name)
                        .Select(subGroup => new SubcategoryGroup
                        {
                            SubcategoryName = subGroup.Key,
                            Products = subGroup.ToList()
                        }).ToList()
                });

            foreach (var group in grouped)
                GroupedProducts.Add(group);


            PropertyChanged?.Invoke(this, new(nameof(GroupedProducts)));
        }
    }
}
