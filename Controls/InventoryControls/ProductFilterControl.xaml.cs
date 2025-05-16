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
using POS.Models.Core;

namespace POS
{
    /// <summary>
    /// Interaction logic for ProductFilterControl.xaml
    /// </summary>
    public partial class ProductFilterControl : UserControl
    {
        // DependencyProperty for MVVM command binding
        public static readonly DependencyProperty FilterChangedCommandProperty =
            DependencyProperty.Register(
                nameof(FilterChangedCommand),
                typeof(ICommand),
                typeof(ProductFilterControl));

        public ICommand FilterChangedCommand
        {
            get => (ICommand)GetValue(FilterChangedCommandProperty);
            set => SetValue(FilterChangedCommandProperty, value);
        }

        public ProductFilterControl()
        {
            InitializeComponent();
        }

        // Hook your internal filter logic to execute the bound command
        private void OnInternalFilterChanged(object sender, FilterChangedEventArgs e)
        {
            if (FilterChangedCommand?.CanExecute(e) == true)
                FilterChangedCommand.Execute(e);
        }

    }
    public class FilterChangedEventArgs : EventArgs
    {
        public string TextSearch { get; set; } = string.Empty;
        public string SelectedCategory { get; set; }
        public string SelectedSubcategory { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string SortOption { get; set; }
    }

}
