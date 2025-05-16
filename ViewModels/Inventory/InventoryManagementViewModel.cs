using POS.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        //public ProductFilterViewModel ProductFilter { get; set; }
        //public ProductDisplayViewModel ProductDisplay { get; set; }
        public InventoryEditorViewModel InventoryEditor { get; set; }

        public InventoryManagementViewModel()
        {
            //ProductFilter = new ProductFilterViewModel();
            //ProductDisplay = new ProductDisplayViewModel();
            //InventoryEditor = new InventoryEditorViewModel();

            //ProductFilter.FilterChanged += OnFilterChanged;
            //ProductDisplay.ProductClicked += OnProductClicked;
        }

        private void OnFilterChanged(object? sender, FilterChangedEventArgs e)
        {
            //ProductDisplay.ApplyFilter(e);
        }

        private void OnProductClicked(Product product)
        {
            InventoryEditor.AddProduct(product);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
