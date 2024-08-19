using System.Windows;
using System.Windows.Controls;
using VisualStudioStarter.Business;
using VisualStudioStarter.ObjectModels;
using VisualStudioStarter.ViewModels;
using WPFUIControls;

namespace VisualStudioStarter.Views;

/// <summary>
/// Logica di interazione per SolutionsPage.xam 
/// </summary>
public partial class SolutionsPage
{
    public SolutionPageViewModel VM => (SolutionPageViewModel)DataContext;

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
        OptionsManager.LoadOptions();
        OptionsManager.CanSave = true;
    }

    private void btnPinned_Click(object sender, RoutedEventArgs e)
    {
        if (sender is RBButton { DataContext: Solution sln })
        {
            VM.PinUnPin_Solution(sln);
        }
    }

    private void OpenSolution_OnClick(object sender, RoutedEventArgs e)
    {
        VM.OpenSolution();
    }

    private void OpenSolutionDirectory_OnClick(object sender, RoutedEventArgs e)
    {
        VM.OpenSolutionDirectory();
    }

    private void RemoveSolution_OnClick(object sender, RoutedEventArgs e)
    {
        VM.Remove_Solution();
    }

    private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        var sol = VM.SelectedSolution;

        if (sol is null)
            return;

        OpenDirectoryPinnedMenu.IsEnabled = sol.DirectoryExist;
        OpenDirectoryUnpinnedMenu.IsEnabled = sol.DirectoryExist;
        OpenPropertiesPinnedMenu.IsEnabled = sol.DirectoryExist;
        OpenPropertiesUnpinnedMenu.IsEnabled = sol.DirectoryExist;

        var index = sol.IsPinned ? VM.PinnedSolutions.IndexOf(sol) : VM.Solutions.IndexOf(sol);
        var max = sol.IsPinned ? VM.PinnedSolutions.Count : VM.Solutions.Count;

        MoveUpUnpinnedMenu.IsEnabled = index > 0;
        MoveDownUnpinnedMenu.IsEnabled = index < max-1;
        MoveUpPinnedMenu.IsEnabled = index > 0;
        MoveDownPinnedMenu.IsEnabled = index < max-1;
    }

    private void RemoveAddPinned_OnClick(object sender, RoutedEventArgs e)
    {
        VM.PinUnPin_Solution();
    }

    private void OpenSolutionProperties_OnClick(object sender, RoutedEventArgs e)
    {
        VM.OpenSolutionProperties();
        
        //Application.Current.Shutdown();
    }

    private void MoveDownMenu_OnClick(object sender, RoutedEventArgs e)
    {
        VM.MoveDownSolution();
    }

    private void MoveUpMenu_OnClick(object sender, RoutedEventArgs e)
    {
        VM.MoveUpSolution();
    }
}