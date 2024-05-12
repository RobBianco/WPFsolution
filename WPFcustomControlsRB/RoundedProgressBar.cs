using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CornerRadius = System.Windows.CornerRadius;

namespace WPFcustomControlsRB
{
    internal class ProgressOpacityCornerConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cr)
            {
                return new CornerRadius(cr.TopLeft > 0 ? cr.TopLeft - 1 : 0,
                    cr.TopRight > 0 ? cr.TopRight - 1 : 0,
                    cr.BottomRight > 0 ? cr.BottomRight - 1 : 0,
                    cr.BottomLeft > 0 ? cr.BottomLeft - 1 : 0);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }

    public class RoundedProgressBar : ContentControl
    {
        

        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            nameof(IsIndeterminate), typeof(bool), typeof(RoundedProgressBar), new PropertyMetadata(default(bool)));

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(RoundedProgressBar), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }


        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(RoundedProgressBar), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        static RoundedProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundedProgressBar), new FrameworkPropertyMetadata(typeof(RoundedProgressBar)));
        }
    }
}
