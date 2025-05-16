using POS.Helpers.Relays;
using POS.Models.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace POS.ViewModels.Order
{
    public class OrderPageViewModel : INotifyPropertyChanged
    {
        public string UserRole { get; }

        public ObservableCollection<Product> FilteredProducts { get; }
            = new(ProductDataStore.Products);

        public ObservableCollection<CartItem> CartItems { get; }
            = new();

        public ICommand ApplyFilterCommand { get; }
        public ICommand AddToCartCommand { get; }

        private readonly Action<Page> _navigate;

        public OrderPageViewModel(Action<Page> navigate, string role)
        {
            _navigate = navigate;
            UserRole = role;

            ApplyFilterCommand = new RelayCommand<object>(ApplyFilter);
            AddToCartCommand = new RelayCommand<Product>(OnProductAddedToCart);
        }

        private void ApplyFilter(object? arg)
        {
            var e = arg as FilterChangedEventArgs;
            FilteredProducts.Clear();
            foreach (var prod in ProductDataStore.Products
                                                .Where(p => /* your filter logic */ true))
                FilteredProducts.Add(prod);
        }

        private void OnProductAddedToCart(Product product)
        {
            var existing = CartItems.FirstOrDefault(ci => ci.Product.Name == product.Name);
            if (existing != null)
                existing.Quantity++;
            else
                CartItems.Add(new CartItem(product, 1));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}