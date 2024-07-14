using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPFUIControls
{
    public class VisibilityBooleanConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            parameter is string str && bool.TryParse(str, out var bb)
                ? !bb
                    ? value is bool bbb
                        ? bbb
                            ? Visibility.Visible
                            : Visibility.Collapsed
                        : Visibility.Visible
                    : value is bool bbbb
                        ? bbbb
                            ? Visibility.Collapsed
                            : Visibility.Visible
                        : Visibility.Collapsed
                : value is bool b
                    ? b
                        ? Visibility.Collapsed
                        : Visibility.Visible
                    : Visibility.Collapsed;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Visibility v)
            {
                return v == Visibility.Collapsed;
            }

            return false;
        }
    }
}
