using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using POS.Models.Core;

namespace POS
{
    /// <summary>
    /// Interaction logic for ShoppingCartControl.xaml
    /// </summary>
    public partial class ShoppingCartControl : UserControl
    {
        private ObservableCollection<CartItem> cartItems = new ObservableCollection<CartItem>();
        private Dictionary<string, int> _cartQuantities = new();
        private Dictionary<string, decimal> _products = new();

        // Events (can still be used externally if needed)
        public event Action<Button>? RemoveRequested;
        public event Action<ComboBox>? QuantityUpdated;


        public void AddProductToCart(Product product)
        {
            if (_cartQuantities.ContainsKey(product.Name))
                _cartQuantities[product.Name]++;
            else
                _cartQuantities[product.Name] = 1;

            if (!_products.ContainsKey(product.Name))
                _products[product.Name] = (decimal)product.Price;

            UpdateCartDisplay();
        }

        public void UpdateCartDisplay()
        {
            var updatedCartItems = _cartQuantities.Select(item => new CartItem
            {
                Name = item.Key,
                Price = (double)_products[item.Key],
                Quantity = item.Value
            }).ToList();

            cartItems.Clear();
            foreach (var item in updatedCartItems)
            {
                cartItems.Add(item);
            }

            cartListView.ItemsSource = null;
            cartListView.ItemsSource = cartItems;


            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = _cartQuantities.Sum(item => _products[item.Key] * item.Value);
            lblTotal.Content = $"Total: Rs. {total}";
        }

        public ShoppingCartControl()
        {
            InitializeComponent();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CartItem item)
            {
                if (_cartQuantities.ContainsKey(item.Name))
                    _cartQuantities.Remove(item.Name);

                if (_products.ContainsKey(item.Name))
                    _products.Remove(item.Name);

                UpdateCartDisplay();
                RemoveRequested?.Invoke(btn);
            }
        }

        private void quantityDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is CartItem item)
            {
                if (comboBox.SelectedItem is int newQuantity)
                {
                    if (_cartQuantities.ContainsKey(item.Name))
                    {
                        _cartQuantities[item.Name] = newQuantity;
                        UpdateTotal();
                    }

                    QuantityUpdated?.Invoke(comboBox);
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _cartQuantities.Clear();
            _products.Clear();
            UpdateCartDisplay();
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            ReceiptPrintWindow receiptPrintWindow= new ReceiptPrintWindow(_cartQuantities, _products);
            Debug.WriteLine("Checkout Pressed");
            receiptPrintWindow.ShowDialog();
        }
    }
}
