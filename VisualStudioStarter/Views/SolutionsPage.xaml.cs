﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualStudioStarter.Business;
using VisualStudioStarter.ObjectModels;
using VisualStudioStarter.Utils;
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
        VsStarterOptions.OnOptionsChanged += VsStarterOptionsOnOnOptionsChanged;

        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void VsStarterOptionsOnOnOptionsChanged(object? oldvalue, object? newvalue, string name)
    {
        if (name == nameof(VsStarterOptions.VisualStudioSelected) &&
            newvalue is VisualStudioVersion vs)
        {
            switch (vs)
            {
                case VisualStudioVersion.None:
                    btn2019.IsChecked = false;
                    btn2022.IsChecked = false;
                    btn2022Pre.IsChecked = false;
                    break;
                case VisualStudioVersion.VS2019:
                    btn2019.IsChecked = true;
                    btn2022.IsChecked = false;
                    btn2022Pre.IsChecked = false;
                    break;
                case VisualStudioVersion.VS2022:
                    btn2019.IsChecked = false;
                    btn2022.IsChecked = true;
                    btn2022Pre.IsChecked = false;
                    break;
                case VisualStudioVersion.VS2022Pre:
                    btn2019.IsChecked = false;
                    btn2022.IsChecked = false;
                    btn2022Pre.IsChecked = true;
                    break;
            }
        }
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

    private async void OpenSolution_OnClick(object sender, RoutedEventArgs e)
    {
        await VM.OpenSolution();
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
        MoveDownUnpinnedMenu.IsEnabled = index < max - 1;
        MoveUpPinnedMenu.IsEnabled = index > 0;
        MoveDownPinnedMenu.IsEnabled = index < max - 1;


        OpenWith2019Pinned.Visibility = VM.IsVS2019Installed ? Visibility.Visible : Visibility.Collapsed;
        OpenWith2022Pinned.Visibility = VM.IsVS2022Installed ? Visibility.Visible : Visibility.Collapsed;
        OpenWith2022PrePinned.Visibility = VM.IsVS2022PreInstalled ? Visibility.Visible : Visibility.Collapsed;

        OpenWithPinned.Visibility = GetVisibilityBasedOnChildControls(
            OpenWith2019Pinned.Visibility,
            OpenWith2022Pinned.Visibility,
            OpenWith2022PrePinned.Visibility
        );

        OpenWith2019Unpinned.Visibility = VM.IsVS2019Installed ? Visibility.Visible : Visibility.Collapsed;
        OpenWith2022Unpinned.Visibility = VM.IsVS2022Installed ? Visibility.Visible : Visibility.Collapsed;
        OpenWith2022PreUnpinned.Visibility = VM.IsVS2022PreInstalled ? Visibility.Visible : Visibility.Collapsed;

        OpenWithUnpinned.Visibility = GetVisibilityBasedOnChildControls(
            OpenWith2019Unpinned.Visibility,
            OpenWith2022Unpinned.Visibility,
            OpenWith2022PreUnpinned.Visibility
        );
    }

    private static Visibility GetVisibilityBasedOnChildControls(params Visibility[] visibilities)
    {
        var visibleCount = visibilities.Count(v => v == Visibility.Visible);
        return visibleCount >= 2 ? Visibility.Visible : Visibility.Collapsed;
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

    private void BtnVS_OnUnchecked(object sender, RoutedEventArgs e)
    {

    }

    private void BtnVS_OnChecked(object sender, RoutedEventArgs e)
    {
        if (Equals(sender, btn2019))
        {
            btn2022.IsChecked = false;
            btn2022Pre.IsChecked = false;
            OptionsManager.Instance.Options.VisualStudioSelected = VisualStudioVersion.VS2019;
        }
        else if (Equals(sender, btn2022))
        {
            btn2019.IsChecked = false;
            btn2022Pre.IsChecked = false;
            OptionsManager.Instance.Options.VisualStudioSelected = VisualStudioVersion.VS2022;
        }
        else if (Equals(sender, btn2022Pre))
        {
            btn2019.IsChecked = false;
            btn2022.IsChecked = false;
            OptionsManager.Instance.Options.VisualStudioSelected = VisualStudioVersion.VS2022Pre;
        }
    }

    private async void OpenWith_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem mi &&
            Enum.TryParse<VisualStudioVersion>(string.Join("", mi.Header.ToString()?.Replace(" ", "").Take(6) ?? Array.Empty<char>()), out var vsVersion))
        {
            if (await VM.OpenSolution(vsVersion: vsVersion))
            {
                Application.Current.Shutdown();
            }
        }
    }

    private async void OnSolution_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
            return;

        if (await VM.OpenSolution())
        {
            Application.Current.Shutdown();
        }
    }
}