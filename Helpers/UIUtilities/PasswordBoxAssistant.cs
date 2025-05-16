using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace POS.Helpers.UIUtilities
{
    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword =
            DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false));

        public static void SetBindPassword(DependencyObject dp, bool value) =>
            dp.SetValue(BindPassword, value);

        public static bool GetBindPassword(DependencyObject dp) =>
            (bool)dp.GetValue(BindPassword);

        public static string GetBoundPassword(DependencyObject dp) =>
            (string)dp.GetValue(BoundPassword);

        public static void SetBoundPassword(DependencyObject dp, string value) =>
            dp.SetValue(BoundPassword, value);

        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox && !GetUpdatingPassword(passwordBox))
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                passwordBox.Password = (string)e.NewValue;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox)
            {
                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
                else
                {
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            SetUpdatingPassword(passwordBox, true);
            SetBoundPassword(passwordBox, passwordBox?.Password);
            SetUpdatingPassword(passwordBox, false);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value) =>
            dp.SetValue(UpdatingPassword, value);

        private static bool GetUpdatingPassword(DependencyObject dp) =>
            (bool)dp.GetValue(UpdatingPassword);
    }
}