﻿using System.Windows;
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
    #region PROPS

    public SolutionPageViewModel VM => (SolutionPageViewModel)DataContext;

    #endregion

    #region CTOR

    public SolutionsPage()
    {
        VsStarterOptions.OnOptionsChanged += VsStarterOptionsOnOnOptionsChanged;

        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    #endregion

    #region EVENTS

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

    private void RemoveSolution_OnClick(object sender, RoutedEventArgs e)
    {
        VM.Remove_Solution();
    }

    private void OpenSolutionProperties_OnClick(object sender, RoutedEventArgs e)
    {
        VM.OpenSolutionProperties();
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
        switch (OptionsManager.Instance.Options.VisualStudioSelected)
        {
            case VisualStudioVersion.None:
                break;
            case VisualStudioVersion.VS2019 when Equals(sender, btn2019) && btn2019.IsChecked is false:
            case VisualStudioVersion.VS2022 when Equals(sender, btn2022) && btn2022.IsChecked is false:
            case VisualStudioVersion.VS2022Pre when Equals(sender, btn2022Pre) && btn2022Pre.IsChecked is false:
                OptionsManager.Instance.Options.VisualStudioSelected = VisualStudioVersion.None;
                break;
        }
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

        if (sender is FrameworkElement { DataContext: Solution clickedSolution })
        {
            if (await VM.OpenSolution(clickedSolution) &&
                !Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                OnOpenSolution?.Invoke(this, clickedSolution);
            }
        }
    }

    private async void OpenCodeSolution_OnClick(Object sender, RoutedEventArgs e)
    {
        if (await VM.OpenSolutionWithVSCode() &&
            !Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
        {
            OnOpenSolution?.Invoke(this, VM.SelectedSolution);
        }
    }

    internal event EventHandler<Solution?>? OnOpenSolution;

    private void OpenSolutionDirectory_OnClick(object sender, RoutedEventArgs e)
    {
        VM.OpenSolutionDirectory();
    }

    #endregion

    #region METHODS

    private static Visibility GetVisibilityBasedOnChildControls(params Visibility[] visibilities)
    {
        var visibleCount = visibilities.Count(v => v == Visibility.Visible);
        return visibleCount >= 2 ? Visibility.Visible : Visibility.Collapsed;
    }

    #endregion

    private void SetDefaulVs_OnClick(Object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem {Tag: VisualStudioVersion vs})
        {
            VM.SetSetDefaultVS(vs);
        }
    }

    private void ChangePinnedState_OnClick(Object sender, RoutedEventArgs e)
    {
        VM.PinUnPin_Solution();
    }

    private void RenameSolution_OnClick(object sender, RoutedEventArgs e)
    {
        VM.RenameSolution();
    }
}