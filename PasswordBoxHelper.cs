using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace POS
{
    internal class PasswordBoxHelper
    {
        public static string GetPlaceholder(DependencyObject obj) =>
            (string)obj.GetValue(PlaceholderProperty);

        public static void SetPlaceholder(DependencyObject obj, string value) =>
            obj.SetValue(PlaceholderProperty, value);

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: OnPlaceholderChanged)
            );

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                if (!passwordBox.IsLoaded)
                {
                    passwordBox.Loaded -= PasswordBox_Loaded;
                    passwordBox.Loaded += PasswordBox_Loaded;
                }

                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;

                if (GetOrCreateAdorner(passwordBox, out var adorner))
                    adorner.InvalidateVisual();
            }
        }

        private static void PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.Loaded -= PasswordBox_Loaded;
                GetOrCreateAdorner(passwordBox, out _);
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox &&
                GetOrCreateAdorner(passwordBox, out var adorner))
            {
                adorner.Visibility = string.IsNullOrEmpty(passwordBox.Password)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            }
        }

        private static bool GetOrCreateAdorner(PasswordBox passwordBox, out PlaceholderAdorner adorner)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(passwordBox);

            if (layer == null)
            {
                adorner = null!;
                return false;
            }

            adorner = layer.GetAdorners(passwordBox)?.OfType<PlaceholderAdorner>().FirstOrDefault()!;

            if (adorner == null)
            {
                adorner = new PlaceholderAdorner(passwordBox);
                layer.Add(adorner);
            }

            return true;
        }

        private class PlaceholderAdorner : Adorner
        {
            private readonly PasswordBox _passwordBox;

            public PlaceholderAdorner(PasswordBox passwordBox)
                : base(passwordBox)
            {
                _passwordBox = passwordBox;
                IsHitTestVisible = false;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                string placeholder = GetPlaceholder(_passwordBox);
                if (string.IsNullOrEmpty(placeholder) || !string.IsNullOrEmpty(_passwordBox.Password))
                    return;

                var formattedText = new FormattedText(
                    placeholder,
                    System.Globalization.CultureInfo.CurrentCulture,
                    _passwordBox.FlowDirection,
                    new Typeface(_passwordBox.FontFamily, _passwordBox.FontStyle, _passwordBox.FontWeight, _passwordBox.FontStretch),
                    _passwordBox.FontSize,
                    Brushes.Gray,
                    VisualTreeHelper.GetDpi(_passwordBox).PixelsPerDip);

                var padding = _passwordBox.Padding;
                var point = new Point(padding.Left + 2, padding.Top + 2);
                drawingContext.DrawText(formattedText, point);
            }
        }
    }
}
