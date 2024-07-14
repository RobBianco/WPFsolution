using System.Windows;
using System.Windows.Controls;
using CornerRadius = System.Windows.CornerRadius;

namespace WPFUIControls
{
    public class RBProgressBar : ContentControl
    {
        

        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            nameof(IsIndeterminate), typeof(bool), typeof(RBProgressBar), new PropertyMetadata(default(bool)));

        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(RBProgressBar), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(RBProgressBar), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        static RBProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RBProgressBar), new FrameworkPropertyMetadata(typeof(RBProgressBar)));
        }
    }
}
