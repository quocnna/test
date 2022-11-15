using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoTest.Core
{
    public static class UIHelper
    {
        public static T GetAncestor<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
            while (parent != null && !(parent is T))
                parent = VisualTreeHelper.GetParent(parent);
            return parent as T;
        }

        public static readonly DependencyProperty BringIntoViewProperty =
            DependencyProperty.RegisterAttached("BringIntoView",
            typeof(bool),
            typeof(UIHelper),
            new PropertyMetadata(OnBringIntoViewChanged));

        public static void SetBringIntoView(DependencyObject o, bool value)
        {
            o.SetValue(BringIntoViewProperty, value);
        }

        public static bool GetBringIntoView(DependencyObject o)
        {

            return (bool)o.GetValue(BringIntoViewProperty);
        }

        private static void OnBringIntoViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (d is FrameworkElement)
                    ((FrameworkElement)d).BringIntoView(new Rect(0, 0, 50, 50));
            }
        }
    }
}
