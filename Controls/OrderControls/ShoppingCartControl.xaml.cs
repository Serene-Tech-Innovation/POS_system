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
        // ItemsSource DP for binding the cart items
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<CartItem>),
                typeof(ShoppingCartControl));

        public IEnumerable<CartItem> ItemsSource
        {
            get => (IEnumerable<CartItem>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public ShoppingCartControl()
        {
            InitializeComponent();
        }
    }
}