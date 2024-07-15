using System.IO;
using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using VisualStudioStarter.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using VisualStudioStarter.Business;

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

    private void btnVs_Click(object sender, RoutedEventArgs e)
    {
        if (VM.AvviaVisualStudio())
        {
            Close();
        }
    }

    public void SetHeight()
    {
        Animate(Height, 120 + Convert.ToDouble(VM.Solutions.Count * 32), new PropertyPath(HeightProperty), "HEIGHT");
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