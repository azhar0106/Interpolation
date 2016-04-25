using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CartesianDraw.View
{
    public static class ViewBehaviors
    {
        #region SetMinSizeAsLoadedSize
        
        public static bool GetSetMinSizeAsLoadedSize(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetMinSizeAsLoadedSizeProperty);
        }

        public static void SetSetMinSizeAsLoadedSize(DependencyObject obj, bool value)
        {
            obj.SetValue(SetMinSizeAsLoadedSizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for SetMinSizeAsLoadedSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetMinSizeAsLoadedSizeProperty =
            DependencyProperty.RegisterAttached("SetMinSizeAsLoadedSize", typeof(bool), typeof(Window), new PropertyMetadata(false, SetMinSizeAsLoadedSizeChanged));
        
        private static void SetMinSizeAsLoadedSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = d as Window;
            if (w == null) return;

            if (GetSetMinSizeAsLoadedSize(w) == true)
            {
                w.Loaded -= Window_Loaded;
                w.Loaded += Window_Loaded;
            }
            else
            {
                w.Loaded -= Window_Loaded;
            }
        }

        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window w = sender as Window;
            if (w == null) return;

            Size s = w.RenderSize;
            w.MinWidth = s.Width;
            w.MinHeight = s.Height;

            w.Loaded -= Window_Loaded;
        }

        #endregion
    }
}
