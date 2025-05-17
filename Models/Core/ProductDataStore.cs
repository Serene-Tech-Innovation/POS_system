using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace POS.Models.Core
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; } = null!;
        public Subcategory Subcategory { get; set; } = null!;
        public string ImagePath { get; set; } = string.Empty;

        public ImageSource? ImageSource
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ImagePath) || !File.Exists(ImagePath))
                    return null;

                try
                {
                    return new BitmapImage(new Uri(ImagePath, UriKind.Absolute));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }

    public class Category
    {
        public string Name { get; set; } = string.Empty;
        public List<Subcategory> Subcategories { get; set; } = new();
    }

    public class Subcategory
    {
        public string Name { get; set; } = string.Empty;
        public Category ParentCategory { get; set; } = null!;
    }

    public class CategoryGroup
    {
        public string CategoryName { get; set; } = string.Empty;
        public List<SubcategoryGroup> Subcategories { get; set; } = new();
    }

    public class SubcategoryGroup
    {
        public string SubcategoryName { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
    }

    public static class ProductDataStore
    {
        public static List<Product> Products { get; } = new();
        public static List<Category> Categories { get; } = new();

        /// <summary>Retrieves a product by name (case-insensitive).</summary>
        public static Product? GetProduct(string name) =>
            Products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        /// <summary>Adds a new product or updates an existing one.</summary>
        public static void SetProduct(Product product)
        {
            var existing = GetProduct(product.Name);
            if (existing == null)
            {
                Products.Add(product);
            }
            else
            {
                existing.Price = product.Price;
                existing.Stock = product.Stock;
                existing.Category = product.Category;
                existing.Subcategory = product.Subcategory;
                existing.ImagePath = product.ImagePath;
            }
        }

        /// <summary>Retrieves a category by name (case-insensitive).</summary>
        public static Category? GetCategory(string name) =>
            Categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        /// <summary>Adds a new category or updates an existing one.</summary>
        public static void SetCategory(Category category)
        {
            var existing = GetCategory(category.Name);
            if (existing == null)
            {
                Categories.Add(category);
            }
            else
            {
                if (!Enumerable.SequenceEqual(existing.Subcategories, category.Subcategories))
                    existing.Subcategories = category.Subcategories;
            }
        }

        /// <summary>Retrieves a subcategory by category and subcategory name (case-insensitive).</summary>
        public static Subcategory? GetSubcategory(string categoryName, string subcategoryName)
        {
            var category = GetCategory(categoryName);
            return category?.Subcategories.FirstOrDefault(s =>
                s.Name.Equals(subcategoryName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Adds or updates a subcategory within a category.</summary>
        public static void SetSubcategory(string categoryName, Subcategory subcategory)
        {
            var category = GetCategory(categoryName);
            if (category == null)
            {
                category = new Category { Name = categoryName };
                Categories.Add(category);
            }

            var existing = category.Subcategories.FirstOrDefault(s =>
                s.Name.Equals(subcategory.Name, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
            {
                subcategory.ParentCategory = category;
                category.Subcategories.Add(subcategory);
            }
            else
            {
                existing.ParentCategory = category;
                // Extend if more properties are added
            }
        }

        /// <summary>Clears all products and categories.</summary>
        public static void ClearAll()
        {
            Products.Clear();
            Categories.Clear();
        }
    }
}
