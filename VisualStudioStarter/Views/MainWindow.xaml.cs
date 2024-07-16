using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using VisualStudioStarter.ViewModels;
using VisualStudioStarter.ObjectModels;
using WPFUIControls;

namespace VisualStudioStarter.Views;

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


    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SetHeight();
    }

    private void Animate(double from, double to, PropertyPath propPath, string name = "")
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
        if (name != "")
        {
            storyboard.Completed += StoryboardOnCompleted;
        }
        

        storyboard.Children.Add(animation);
        Storyboard.SetTarget(animation, this);
        Storyboard.SetTargetProperty(animation, propPath);

        // Inizia l'animazione
        storyboard.Begin();
    }

    private void StoryboardOnCompleted(object? sender, EventArgs e)
    {
        Animate(Top, SystemParameters.PrimaryScreenHeight - Height - 45, new PropertyPath(TopProperty));
        Animate(0, 1, new PropertyPath(OpacityProperty));
    }

    public void SetHeight()
    {
        Animate(Height, 190 + Convert.ToDouble(VM.Solutions.Count * 32), new PropertyPath(HeightProperty), "HEIGHT");
    }


    private void btnAddSolutionFolder_Click(object sender, RoutedEventArgs e)
    {
        VM.ChooseSolutions(true);
        SetHeight();
    }

    private void btnAddSolution_Click(object sender, RoutedEventArgs e)
    {
        VM.ChooseSolutions(false);
        SetHeight();
    }

    private void btnPinned_Click(object sender, RoutedEventArgs e)
    {
        if (sender is RBButton { DataContext: Solution sln })
        {
            VM.PinUnPin_Solution(sln);
        }
    }

    private void DragWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void OnSolution_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            PopupSolution.IsOpen = !PopupSolution.IsOpen;
            return;
        }

        var sln = e.Source is FrameworkElement { DataContext: Solution slnn } ? slnn : null;

        if (VM.AvviaVisualStudio(sln))
        {
            Close();
        }
    }
}