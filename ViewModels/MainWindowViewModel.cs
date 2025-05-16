using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace POS.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly Frame _mainFrame;

        public MainWindowViewModel(Frame frame)
        {
            _mainFrame = frame;
            NavigateToLogin();
        }

        public void NavigateToLogin()
        {
            var loginPage = new Login(page => _mainFrame.Navigate(page));
            _mainFrame.Navigate(loginPage);
        }
    }
}
