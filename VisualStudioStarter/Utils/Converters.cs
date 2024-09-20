using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VisualStudioStarter.Utils
{
    internal class DefaultVsToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc />
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var res = System.Windows.Visibility.Collapsed;
            if (value is VisualStudioVersion vs)
            {
                res = vs switch
                {
                    VisualStudioVersion.None => System.Windows.Visibility.Collapsed,
                    _ => System.Windows.Visibility.Visible
                };
            }

            return parameter is "reverse" ? res == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed : res;
        }

        /// <inheritdoc />
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
