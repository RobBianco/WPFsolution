using System.Windows;
using System.Windows.Controls;
using CornerRadius = System.Windows.CornerRadius;

namespace WPFcustomControlsRB
{
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
