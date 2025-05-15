using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static POS.OrderPage;

namespace POS
{
    /// <summary>
    /// Interaction logic for ShoppingCartControl.xaml
    /// </summary>
    public partial class ShoppingCartControl : UserControl
    {
        // Event that passes CartItem to the parent
        public event Action<Button>? RemoveRequested;
        public event Action<ComboBox>? QuantityUpdated;

        public ShoppingCartControl()
        {
            InitializeComponent();
        }

        private void quantityDropDown_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is CartItem item)
            {
                int newQuantity = (int)comboBox.SelectedItem;

                // Raise event to be handled in OrderPage
                QuantityUpdated?.Invoke(comboBox);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CartItem item)
            {
                // Raise event to be handled in OrderPage
                RemoveRequested?.Invoke(btn);
            }
        }
        
    }
}
