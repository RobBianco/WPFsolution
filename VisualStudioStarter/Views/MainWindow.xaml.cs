using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using VisualStudioStarter.ViewModels;
using VisualStudioStarter.Business;
using VisualStudioStarter.ObjectModels;
using VisualStudioStarter.Utils;
using Timer = System.Timers.Timer;

namespace VisualStudioStarter.Views;

public partial class MainWindow
{
    private SolutionsPage? _solutionsPage;
    private Storyboard _TopStoryBoard;
    private Storyboard _OpacityStoryBoard;
    private Storyboard _HeightStoryBoard;
    private Storyboard _WidthStoryBoard;
    private Storyboard _LeftStoryBoard;
    private double _d;
    private bool _initialize;
    private readonly GlobalKeyboardHook _globalKeys = new();
    public MainViewModel VM => (MainViewModel)DataContext;
    public SolutionsPage SolutionsPage => _solutionsPage ??= new();
    public Timer Timer { get; set; } = new();

    public MainWindow()
    {
        _initialize = true;
        VsStarterOptions.OnOptionsChanged += VsStarterOptionsOnOnOptionsChanged;
        _globalKeys.KeyPressed += GlobalKeysOnKeyPressed;

        InitializeComponent();
        InitializeControls();
        Loaded += OnLoaded;
        Closing += OnClosing;
        Frame.Content = SolutionsPage;

        Left = GetLeft();
        Opacity = 0;
        Top = SystemParameters.PrimaryScreenHeight;
    }

    private void GlobalKeysOnKeyPressed(object? sender, KeyPressedEventArgs e)
    {
        if (e.VirtualKeyCode == 27)  // 27 è il codice virtuale per Esc
        {
            try
            {
                if (DialogHost.GetDialogSession("SolutionDialogHost") is not null
                    || DialogHost.GetDialogSession("MainDialogHost") is not null)
                {
                    return;
                }
            }
            catch (Exception)
            {
                // continue
            }
            

            Application.Current.Shutdown();
        }
    }

    private void InitializeControls()
    {
        Timer.Elapsed += TimerOnElapsed;
        Timer.Interval = 300;

        TextBoxWidth.Text = OptionsManager.Instance.Options.Width.ToString("");
        ComboBoxPinnedPlacement.SelectedItem = OptionsManager.Instance.Options.PinnedPlacement;
        ComboBoxStartingPosition.SelectedItem = OptionsManager.Instance.Options.StartPosition;
        ToggleTopMost.IsChecked = OptionsManager.Instance.Options.TopMost;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.Invoke(() =>
            AnimateWidth(_d, new Duration(TimeSpan.FromMilliseconds(400)),
                onCompleted: (_, _) => AnimateLeft(new Duration(TimeSpan.FromMilliseconds(200)))));

    }

    #region ANIMATION

    public double GetHeight()
    {
        const double mainborderMargins = 20;
        const double listboxPadding = 10 * 2;
        const double btnVSPagging = 10;
        const double margins = mainborderMargins + listboxPadding + btnVSPagging;
        const double titleBar = 45;
        const double scritta = 16;
        const double item = 31;
        const double vsbuttons = 60;
        var h1 = SolutionsPage.VM.PinnedSolutions.Count * item;
        var h2 = SolutionsPage.VM.Solutions.Count * item;
        var h11 = h1 > 0 ? scritta + h1 : 0;
        var h22 = h2 > 0 ? scritta + h2 : 0;
        var h = margins + titleBar + h11 + h22 + vsbuttons;
        return Math.Min(MaxHeight, Math.Max(300,h));
    }

    public double GetTop() => SystemParameters.PrimaryScreenHeight - GetHeight() - 45;
    public double GetLeft() =>
        OptionsManager.Instance.Options.StartPosition switch
        {
            StartPosition.Center => (SystemParameters.PrimaryScreenWidth - ActualWidth) / 2,
            StartPosition.Left => 10,
            StartPosition.Right => SystemParameters.PrimaryScreenWidth - ActualWidth - 10,
            _ => (SystemParameters.PrimaryScreenWidth - ActualWidth) / 2
        };

    public void AnimateTop(Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(ref _TopStoryBoard, Top, GetTop(), new(TopProperty), duration, onCompleted);
    }

    public void AnimateOpacity(Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(ref _OpacityStoryBoard, 0, 1, new(OpacityProperty), duration, onCompleted);
    }

    public void AnimateHeight(Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(ref _HeightStoryBoard, Height, GetHeight(), new(HeightProperty), duration, onCompleted);
    }
    public void AnimateWidth(double width, Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(ref _WidthStoryBoard,ActualWidth, width, new(WidthProperty), duration, onCompleted);
    }

    public void AnimateLeft(Duration? duration = null, EventHandler? onCompleted = null)
    {
        Animate(ref _LeftStoryBoard, Left, GetLeft(), new(LeftProperty), duration, onCompleted);
    }


    private void Animate(ref Storyboard storyboard, double from, double to, PropertyPath propPath, Duration? duration = null, EventHandler? onCompleted = null)
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
        storyboard = new()
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

    private void VsStarterOptionsOnOnOptionsChanged(object? oldvalue, object? newvalue, string name)
    {
        if (name == nameof(VsStarterOptions.TopMost) && newvalue is bool b)
        {
            Topmost = b;
        }

        if (name == nameof(VsStarterOptions.Width) && newvalue is double d)
        {
            _d = d;
            if (!OptionsManager.CanSave)
            {
                Width = d;
                Left = GetLeft();
                return;
            }

            Timer.AutoReset = false;
            Timer.Stop();
            Timer.Start();
        }

        if (name == nameof(VsStarterOptions.StartPosition))
        {
            if (!OptionsManager.CanSave)
            {
                Left = GetLeft();
                return;
            }

            AnimateLeft();
        }
    }

    private void OnSolutionPinnedUnPinned(object? sender, EventArgs e)
    {
        AnimateTop(new Duration(TimeSpan.FromMilliseconds(300)));
        AnimateHeight(new Duration(TimeSpan.FromMilliseconds(300)));
    }

    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        _globalKeys.Dispose();
        SolutionsPage.VM.SaveSolutions();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AnimateOpacity();
        AnimateTop();
        AnimateHeight();

        SolutionsPage.VM.OnSolutionPinnedUnPinned += OnSolutionPinnedUnPinned;
        _initialize = false;
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

    private async void btnAddSolutionFolder_Click(object sender, RoutedEventArgs e)
    {
        await SolutionsPage.VM.AddSolutions(await SolutionManager.FindSolution(true));
        DialogHost.GetDialogSession("MainDialogHost")?.Close();
        AnimateHeight();
        AnimateTop();
        VM.IsOptionsDrawerOpen = false;
    }

    private async void btnAddSolution_Click(object sender, RoutedEventArgs e)
    {
        await SolutionsPage.VM.AddSolutions(await SolutionManager.FindSolution(false));
        DialogHost.GetDialogSession("MainDialogHost")?.Close();
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

    private void AlwaysOnTop_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ToggleTopMost.IsChecked = !ToggleTopMost.IsChecked;
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ToggleTopMost_OnChecked(object sender, RoutedEventArgs e)
    {
        OptionsManager.Instance.Options.TopMost = ToggleTopMost.IsChecked ?? false;
    }

    private void ToggleTopMost_OnUnchecked(object sender, RoutedEventArgs e)
    {
        OptionsManager.Instance.Options.TopMost = ToggleTopMost.IsChecked ?? false;
    }

    private void TextBoxWidth_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (double.TryParse(TextBoxWidth.Text, out var result))
        {
            if (result > MaxWidth)
            {
                result = MaxWidth;
            }
            else if (result < MinWidth)
            {
                result = MinWidth;
            }

            OptionsManager.Instance.Options.Width = result;
        }
    }

    private void TextBoxWidth_OnKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;

        if (e.Key is Key.Enter or Key.Tab)
        {
            ComboBoxPinnedPlacement.Focus();
            return;
        }

        if (e.Key is >= Key.D0 and <= Key.D9 or >= Key.NumPad0 and Key.NumPad9)
        {
            e.Handled = false;
        }
    }

    private void TextBoxWidth_OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (!double.TryParse(TextBoxWidth.Text, out var result)) return;

        TextBoxWidth.TextChanged -= TextBoxWidth_OnTextChanged;
        if (result > MaxWidth)
        {
            TextBoxWidth.Text = MaxWidth.ToString("");
        }
        else if (result < MinWidth)
        {
            TextBoxWidth.Text = MinWidth.ToString("");
        }
        TextBoxWidth.TextChanged += TextBoxWidth_OnTextChanged;
    }

    private void ComboBoxPinnedPlacement_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ComboBoxPinnedPlacement.SelectedItem is PinnedPlacement pp)
        {
            OptionsManager.Instance.Options.PinnedPlacement = pp;
        }
    }
    private void ComboBoxStartingPosition_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ComboBoxStartingPosition.SelectedItem is StartPosition sp)
        {
            OptionsManager.Instance.Options.StartPosition = sp;
        }
    }
}