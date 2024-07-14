using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPFUIControls
{
    public class RBToggleButton : ToggleButton
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(RBToggleButton), new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty MouseHoverBackColorProperty = DependencyProperty.Register(
            nameof(MouseHoverBackColor), typeof(Brush), typeof(RBToggleButton), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public static readonly DependencyProperty CheckedBackColorProperty = DependencyProperty.Register(
            nameof(CheckedBackColor), typeof(Brush), typeof(RBToggleButton), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty HorizontalPictureAlignmentProperty = DependencyProperty.Register(
            nameof(HorizontalPictureAlignment), typeof(HorizontalAlignment), typeof(RBToggleButton), new PropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty VerticalPictureAlignmentProperty = DependencyProperty.Register(
            nameof(VerticalPictureAlignment), typeof(HorizontalAlignment), typeof(RBToggleButton), new PropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty PictureStretchProperty = DependencyProperty.Register(
            nameof(PictureStretch), typeof(Stretch), typeof(RBToggleButton), new PropertyMetadata(default(Stretch)));

        public static readonly DependencyProperty PictureProperty = DependencyProperty.Register(
            nameof(Picture), typeof(ImageSource), typeof(RBToggleButton), new PropertyMetadata(default(ImageSource)));

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

        public ImageSource Picture
        {
            get => (ImageSource)GetValue(PictureProperty);
            set => SetValue(PictureProperty, value);
        }

        public HorizontalAlignment HorizontalPictureAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalPictureAlignmentProperty);
            set => SetValue(HorizontalPictureAlignmentProperty, value);
        }

        public HorizontalAlignment VerticalPictureAlignment
        {
            get => (HorizontalAlignment)GetValue(VerticalPictureAlignmentProperty);
            set => SetValue(VerticalPictureAlignmentProperty, value);
        }

        public Stretch PictureStretch
        {
            get => (Stretch)GetValue(PictureStretchProperty);
            set => SetValue(PictureStretchProperty, value);
        }
    }
}