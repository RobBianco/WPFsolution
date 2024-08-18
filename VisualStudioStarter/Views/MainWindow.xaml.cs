using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using VisualStudioStarter.ViewModels;
using VisualStudioStarter.Business;
using VisualStudioStarter.ObjectModels;

namespace VisualStudioStarter.Views;

public partial class MainWindow
{
    private SolutionsPage? _solutionsPage;
    public MainViewModel VM => (MainViewModel)DataContext;
    public SolutionsPage SolutionsPage => _solutionsPage ??= new SolutionsPage();
    public VsStarterOptions VsStarterOptions { get; } = OptionsManager.GetOptions();

    public MainWindow()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Closing += OnClosing;
        VsStarterOptions.OnOptionsChanged += VsStarterOptionsOnOnOptionsChanged;

        Opacity = 0;
        Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        Top = SystemParameters.PrimaryScreenHeight;
        Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;

#if DEBUG
        Left = (SystemParameters.PrimaryScreenWidth - Width);
        Topmost = true;
#endif
        Frame.Content = SolutionsPage;
        if (SolutionsPage.VM != null)
            SolutionsPage.VM.OnSolutionPinnedUnPinned += OnSolutionPinnedUnPinned;
    }

    #region ANIMATION

    public void AnimateTop(Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(Top, SystemParameters.PrimaryScreenHeight - GetHeight() - 45, new PropertyPath(TopProperty), duration, onCompleted);
    }

    public void AnimateOpacity(Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(0, 1, new PropertyPath(OpacityProperty), duration, onCompleted);
    }

    public int GetHeight()
    {
        const int mainborderMargins = 20;
        const int listboxPadding = 10 * 2;
        const int btnVSPagging = 10;
        const int margins = mainborderMargins + listboxPadding + btnVSPagging;
        const int titleBar = 45;
        const int scritta = 16;
        const int item = 31;
        const int vsbuttons = 60;
        var h1 = SolutionsPage.VM?.PinnedSolutions.Count * item ?? 0;
        var h2 = SolutionsPage.VM?.Solutions.Count * item ?? 0;
        var h11 = h1 > 0 ? scritta + h1 : 0;
        var h22 = h2 > 0 ? scritta + h2 : 0;
        var h = margins + titleBar + h11 + h22 + vsbuttons;
        return Math.Max(200, h);
    }

    public void AnimateHeight(Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(Height, GetHeight(), new PropertyPath(HeightProperty), duration, onCompleted);
    }

    private void Animate(double from, double to, PropertyPath propPath, Duration? duration = null, EventHandler? onCompleted = null)
    {
        if (from == to) return;

        var animation = new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = duration ?? new Duration(TimeSpan.FromMilliseconds(700)),
            EasingFunction = new SineEase()
            {
                EasingMode = EasingMode.EaseInOut
            },
        };

        // Crea un Storyboard per contenere l'animazione
        var storyboard = new Storyboard
        {
            Children = { animation }
        };
        if (onCompleted is not null)
            storyboard.Completed += onCompleted;
        Storyboard.SetTarget(animation, this);
        Storyboard.SetTargetProperty(animation, propPath);

        // Inizia l'animazione
        storyboard.Begin();
    }

    #endregion ANIMATION

    private void VsStarterOptionsOnOnOptionsChanged(object oldvalue, object newvalue, string name)
    {
        OptionsManager.SaveOptions(VsStarterOptions);

        SolutionsPage.VM?.VsStarterOptionsOnOnOptionsChanged(oldvalue, newvalue, name);
    }

    private void OnSolutionPinnedUnPinned(object? sender, EventArgs e)
    {
        AnimateTop(new Duration(TimeSpan.FromMilliseconds(100)));
        AnimateHeight(new Duration(TimeSpan.FromMilliseconds(0)));
    }

    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        SolutionsPage.VM?.SaveSolutions();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AnimateOpacity();
        AnimateTop();
        AnimateHeight();
    }

    private void DragWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        catch (Exception)
        {
            // continue
        }
    }

    private void btnAddSolutionFolder_Click(object sender, RoutedEventArgs e)
    {
        if (SolutionsPage.VM == null)
            return;

        SolutionsPage.VM.AddSolutions(SolutionManager.FindSolution(true));
        AnimateHeight();
        AnimateTop();
        VM.IsOptionsDrawerOpen = false;
    }

    private void btnAddSolution_Click(object sender, RoutedEventArgs e)
    {
        if (SolutionsPage.VM == null)
            return;

        SolutionsPage.VM.AddSolutions(SolutionManager.FindSolution(false));
        AnimateHeight();
        AnimateTop();
        VM.IsOptionsDrawerOpen = false;
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right && Keyboard.Modifiers.HasFlag(ModifierKeys.Control | ModifierKeys.Shift))
        {
            AnimateHeight();
            AnimateTop();
        }
    }
}