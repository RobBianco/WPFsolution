using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using VisualStudioStarter.ViewModels;
using VisualStudioStarter.Business;

namespace VisualStudioStarter.Views;

public partial class MainWindow
{
    private OptionsPage? _optionsPage;
    private SolutionsPage? _solutionsPage;
    public MainViewModel VM => (MainViewModel)DataContext;
    public OptionsPage OptionsPage => _optionsPage ??= new OptionsPage();
    public SolutionsPage SolutionsPage => _solutionsPage ??= new SolutionsPage(VM);

    public MainWindow()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Closing += OnClosing;
        Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        Top = SystemParameters.PrimaryScreenHeight;
        Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
    }

    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        SolutionManager.SaveSolutions(VM.Solutions.ToList());
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SetHeight();

        Frame.Content = SolutionsPage;
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

    private void DragWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }


    private void OpenSetting_OnClick(object sender, RoutedEventArgs e)
    {
        Frame.Content = Equals(Frame.Content, OptionsPage) ? SolutionsPage : OptionsPage;
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
}