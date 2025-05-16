using POS.Models.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.ViewModels
{
    public class InventoryEditorViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EditableProduct> EditableProducts { get; set; } = new();
        public List<Category> CategoryList => ProductDataStore.Categories;

        public void AddProduct(Product product)
        {
            if (EditableProducts.Any(p => p.Name == product.Name)) return;

            EditableProducts.Add(new EditableProduct(product));
        }

        public void ApplyAll()
        {
            foreach (var ep in EditableProducts)
                ProductDataStore.SetProduct(ep.ToProduct());
        }

        public void DeleteProduct(EditableProduct ep)
        {
            var original = ProductDataStore.GetProduct(ep.Name);
            if (original != null)
                ProductDataStore.Products.Remove(original);

            EditableProducts.Remove(ep);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
