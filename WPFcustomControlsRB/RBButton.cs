using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFcustomControlsRB
{   
    public class RBButton : Button
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(RBButton), new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty MouseHoverBackColorProperty = DependencyProperty.Register(
            nameof(MouseHoverBackColor), typeof(Brush), typeof(RBButton), new PropertyMetadata(default(Brush)));

        static RBButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RBButton), new FrameworkPropertyMetadata(typeof(RBButton)));
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