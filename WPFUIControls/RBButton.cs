using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFUIControls;

public class RBButton : Button
{
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(RBButton), new PropertyMetadata(default(CornerRadius)));

    public static readonly DependencyProperty MouseHoverBackColorProperty = DependencyProperty.Register(
        nameof(MouseHoverBackColor), typeof(Brush), typeof(RBButton), new PropertyMetadata(default(Brush)));

    public static readonly DependencyProperty PictureStretchProperty = DependencyProperty.Register(
        nameof(PictureStretch), typeof(Stretch), typeof(RBButton), new PropertyMetadata(default(Stretch)));

    public static readonly DependencyProperty HorizontalPictureAlignmentProperty = DependencyProperty.Register(
        nameof(HorizontalPictureAlignment), typeof(HorizontalAlignment), typeof(RBButton), new PropertyMetadata(default(HorizontalAlignment)));

    public static readonly DependencyProperty VerticalPictureAlignmentProperty = DependencyProperty.Register(
        nameof(VerticalPictureAlignment), typeof(HorizontalAlignment), typeof(RBButton), new PropertyMetadata(default(HorizontalAlignment)));

    public static readonly DependencyProperty PictureProperty = DependencyProperty.Register(
        nameof(Picture), typeof(ImageSource), typeof(RBButton), new PropertyMetadata(default(ImageSource)));

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

    public ImageSource Picture
    {
        get => (ImageSource)GetValue(PictureProperty);
        set => SetValue(PictureProperty, value);
    }

    public static readonly DependencyProperty PictureMarginProperty = DependencyProperty.Register(
        nameof(PictureMargin), typeof(Thickness), typeof(RBButton), new PropertyMetadata(default(Thickness)));

    public Thickness PictureMargin
    {
        get => (Thickness)GetValue(PictureMarginProperty);
        set => SetValue(PictureMarginProperty, value);
    }
}