using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Models.Core
{
    public class CartItem : INotifyPropertyChanged
    {
        public Product Product { get; }
        public string Name { get; private set; }

        private double _price;
        public double Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); OnPropertyChanged(nameof(TotalPrice)); }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); OnPropertyChanged(nameof(TotalPrice)); }
        }

        public double TotalPrice => Price * Quantity;

        // Constructor sets Name, Price, Quantity
        public CartItem(Product product, int quantity)
        {
            Product = product;
            Name = product.Name;
            Price = (double)product.Price;
            Quantity = quantity;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}