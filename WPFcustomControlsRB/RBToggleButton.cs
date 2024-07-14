using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPFcustomControlsRB
{
    public class RBToggleButton : ToggleButton
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(RBToggleButton), new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty MouseHoverBackColorProperty = DependencyProperty.Register(
            nameof(MouseHoverBackColor), typeof(Brush), typeof(RBToggleButton), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CheckedBackColorProperty = DependencyProperty.Register(
            nameof(CheckedBackColor), typeof(Brush), typeof(RBToggleButton), new PropertyMetadata(default(Brush)));

        static RBToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RBToggleButton), new FrameworkPropertyMetadata(typeof(RBToggleButton)));
        }

        public Brush CheckedBackColor
        {
            get => (Brush)GetValue(CheckedBackColorProperty);
            set => SetValue(CheckedBackColorProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Brush MouseHoverBackColor
        {
            get => (Brush)GetValue(MouseHoverBackColorProperty);
            set => SetValue(MouseHoverBackColorProperty, value);
        }
    }
}