using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPFUIControls.Converters
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var a = parameter is not null && bool.TryParse(parameter.ToString(), out var ress) && !ress;
            var tr = !a ? Visibility.Visible : Visibility.Collapsed;
            var fa = !a ? Visibility.Collapsed : Visibility.Visible;
            var res =  value switch
            {
                bool b => b ? tr : fa,
                string s => !string.IsNullOrEmpty(s) ? tr : fa,
                float f => f > 0 ? tr : fa,
                double d => d > 0 ? tr : fa,
                IList l => l.Count > 0 ? tr : fa,
                int i => i > 0 ? tr : fa,
                decimal m => m > 0 ? tr : fa,
                DateTime dt => dt > DateTime.MinValue ? tr : fa,
                IDictionary dict => dict.Count > 0 ? tr : fa,
                IEnumerable e => e.Cast<object>().Any() ? tr : fa,
                _ => tr
            };

            return parameter is "reverse" ? res == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed : res;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
