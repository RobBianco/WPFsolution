using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WPFcustomControlsRB.Converters
{
    internal class OpacityCornerConvert : IValueConverter
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

}
