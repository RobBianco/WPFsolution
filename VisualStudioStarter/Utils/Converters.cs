using System.Globalization;
using System.Windows;
using System.Windows.Data;
using VisualStudioStarter.ViewModels;

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

    internal class PositionToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && parameter is string direction && Application.Current.MainWindow.DataContext is SolutionPageViewModel vm)
            {
                if (direction == "up")
                {
                    return index > 0;
                }
                else if (direction == "down")
                {
                    return index < vm.Solutions.Count - 1;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
