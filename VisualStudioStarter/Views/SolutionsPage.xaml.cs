using System.Windows;
using System.Windows.Controls;
using VisualStudioStarter.ObjectModels;
using VisualStudioStarter.ViewModels;
using WPFUIControls;

namespace VisualStudioStarter.Views;

/// <summary>
/// Logica di interazione per SolutionsPage.xaml
/// </summary>
public partial class SolutionsPage : Page
{
    public MainViewModel? VM => DataContext as MainViewModel;

    public SolutionsPage(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {

    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {

    }
    private void btnPinned_Click(object sender, RoutedEventArgs e)
    {
        if (sender is RBButton { DataContext: Solution sln })
        {
            VM?.PinUnPin_Solution(sln);
        }
    }

    private void OpenSolution_OnClick(object sender, RoutedEventArgs e)
    {

    }

    private void OpenSolutionDirectory_OnClick(object sender, RoutedEventArgs e)
    {

    }
}