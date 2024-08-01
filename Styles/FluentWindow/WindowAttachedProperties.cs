using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Fluent_Control_Lib.Styles.FluentWindow
{
    public static class WindowAttachedProperties
    {
        public static readonly DependencyProperty OnLoadedProperty =
            DependencyProperty.RegisterAttached(
                "OnLoaded",
                typeof(RoutedEventHandler),
                typeof(WindowAttachedProperties),
                new PropertyMetadata(null, OnLoadedChanged));
        public static void SetOnLoaded(DependencyObject element, RoutedEventHandler value)
        {
            element.SetValue(OnLoadedProperty, value);
        }

        public static RoutedEventHandler GetOnLoaded(DependencyObject element)
        {
            return (RoutedEventHandler)element.GetValue(OnLoadedProperty);
        }

        private static void OnLoadedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                element.Loaded += (sender, args) =>
                {
                    var handler = GetOnLoaded(element);
                    handler?.Invoke(sender, args);
                };
            }
        }
    }
}
