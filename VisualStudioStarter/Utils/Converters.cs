using System.Globalization;
using System.Windows.Data;

namespace VisualStudioStarter.Utils
{
    internal class DefaultVsToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc />
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is VisualStudioVersion vs)
            {
                return vs switch
                {
                    VisualStudioVersion.None => System.Windows.Visibility.Collapsed,
                    _ => System.Windows.Visibility.Visible
                };
            }

            return System.Windows.Visibility.Collapsed;
        }

        /// <inheritdoc />
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
