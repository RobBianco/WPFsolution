using System.Windows;
using System.Windows.Controls;
using VisualStudioStarter.ObjectModels;
using VisualStudioStarter.ViewModels;
using WPFUIControls;

namespace VisualStudioStarter.Views;

/// <summary>
/// Logica di interazione per SolutionsPage.xam 
/// </summary>
public partial class SolutionsPage
{
    public SolutionPageViewModel? VM => DataContext as SolutionPageViewModel;

    public SolutionsPage()
    {
        InitializeComponent();
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
        VM?.OpenSolution();
    }

    private void OpenSolutionDirectory_OnClick(object sender, RoutedEventArgs e)
    {
        VM?.OpenSolutionDirectory();
    }

    private void RemoveSolution_OnClick(object sender, RoutedEventArgs e)
    {
        VM?.Remove_Solution();
    }

    private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        OpenDirectoryPinnedMenu.IsEnabled = VM?.SelectedSolution?.DirectoryExist ?? false;
        OpenDirectoryUnpinnedMenu.IsEnabled = VM?.SelectedSolution?.DirectoryExist ?? false;
        OpenPropertiesPinnedMenu.IsEnabled = VM?.SelectedSolution?.DirectoryExist ?? false;
        OpenPropertiesUnpinnedMenu.IsEnabled = VM?.SelectedSolution?.DirectoryExist ?? false;
    }

    private void RemoveAddPinned_OnClick(object sender, RoutedEventArgs e)
    {
        VM?.PinUnPin_Solution();
    }

    private void OpenSolutionProperties_OnClick(object sender, RoutedEventArgs e)
    {
        VM?.OpenSolutionProperties();
    }
}