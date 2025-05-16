using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace POS.Models.Core
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public List<Subcategory> Subcategories { get; set; } = new();
    }

    public class Subcategory
    {
        public string Name { get; set; }
        public Category ParentCategory { get; set; }
    }

    public static class ProductDataStore
    {
        public static List<Product> Products { get; } = new();
        public static List<Category> Categories { get; } = new();

        // Get product by name
        public static Product? GetProduct(string name)
            => Products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        // Add or update product
        public static void SetProduct(Product product)
        {
            var existing = GetProduct(product.Name);
            if (existing is null)
            {
                Products.Add(product);
            }
            else
            {
                existing.Price = product.Price;
                existing.Category = product.Category;
                existing.Subcategory = product.Subcategory;
            }
        }

        // Get category by name
        public static Category? GetCategory(string name)
            => Categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        // Add or update category
        public static void SetCategory(Category category)
        {
            Debug.WriteLine($"Setting category: {category.Name}");
            var existing = GetCategory(category.Name);
            if (existing is null)
            {
                Categories.Add(category);
            }
            else
            {
                // Update subcategories - here we replace, or you could merge if needed
                existing.Subcategories = category.Subcategories;
            }
        }

        // Get subcategory by name inside a category
        public static Subcategory? GetSubcategory(string categoryName, string subcategoryName)
        {
            var category = GetCategory(categoryName);
            return category?.Subcategories.FirstOrDefault(s => s.Name.Equals(subcategoryName, StringComparison.OrdinalIgnoreCase));
        }

        // Add or update subcategory inside a category
        public static void SetSubcategory(string categoryName, Subcategory subcategory)
        {
            var category = GetCategory(categoryName);
            if (category == null)
            {
                // Create category if doesn't exist
                category = new Category { Name = categoryName };
                Categories.Add(category);
            }

            var existingSubcat = category.Subcategories.FirstOrDefault(s => s.Name.Equals(subcategory.Name, StringComparison.OrdinalIgnoreCase));
            if (existingSubcat == null)
            {
                subcategory.ParentCategory = category;
                category.Subcategories.Add(subcategory);
            }
            else
            {
                // Update the ParentCategory reference if needed
                existingSubcat.ParentCategory = category;
                // Here you can update other properties if any in Subcategory
            }
        }

        public static void ClearAll()
        {
            Products.Clear();
            Categories.Clear();
        }
    }
}
