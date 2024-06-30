using System.Windows;
using System.Windows.Media.Animation;

namespace VisualStudioStarter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainViewModel VM => (MainViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();

        Loaded += OnLoaded;

        Top = SystemParameters.PrimaryScreenHeight;
        Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
    }

    private void OnLoaded(Object sender, RoutedEventArgs e)
    {
        VM.SetActiveWorkSpace();

        SetHeight();

        var screenHeight = SystemParameters.PrimaryScreenHeight;
        var topFinal = SystemParameters.PrimaryScreenHeight - Height - 45;
        Animate(screenHeight, topFinal, new PropertyPath(TopProperty));
        Animate(0, 1, new PropertyPath(OpacityProperty));
    }

    private void Animate(double from, double to, PropertyPath propPath, String name = "")
    {
        var animation = new DoubleAnimation
        {
            Name = name,
            From = from,
            To = to,
            Duration = new Duration(TimeSpan.FromMilliseconds(700)),
            EasingFunction = new SineEase()
            {
                EasingMode = EasingMode.EaseInOut
            },
        };

        // Crea un Storyboard per contenere l'animazione
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);
        Storyboard.SetTarget(animation, this);
        Storyboard.SetTargetProperty(animation, propPath);

        // Inizia l'animazione
        storyboard.Begin();
    }

    private void ButtonBase_OnClick(Object sender, RoutedEventArgs e)
    {
        VM.OpenOrAddWorkSpace(this);

        SetHeight();
    }

    private void btnVs_Click(Object sender, RoutedEventArgs e)
    {
        if (VM.AvviaVisualStudio())
        {
            Close();
        }
    }

    public void SetHeight()
    {
        Height = 120 + Convert.ToDouble(VM.ActiveWorkSpace?.Solutions.Count * 31);
        Animate(Top, SystemParameters.PrimaryScreenHeight - Height - 45, new PropertyPath(TopProperty));
    }

    private void TrashButton_OnClick(object sender, RoutedEventArgs e)
    {
        VM.DeleteWorkSpace();
    }
}