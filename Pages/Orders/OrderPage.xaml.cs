using Npgsql;
using POS.Models.Core;
using POS.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Path = System.IO.Path;

namespace POS
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {

        public OrderPage(Action<Page> navigate, string role)
        {
            InitializeComponent();
            DataContext = new OrderPageViewModel(navigate, role);
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

    }
}
