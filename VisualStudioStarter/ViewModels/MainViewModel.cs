using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using VisualStudioStarter.Business;
using VisualStudioStarter.ObjectModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace VisualStudioStarter.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Solution> _solutions = [];
    private bool _isVs2022PreInstalled;
    private bool _isVs2022Installed;
    private bool _isVs2019Installed;
    private bool _isAdmin;
    private bool _isVisualStudio2022Pre;
    private bool _isVisualStudio2022;
    private bool _isVisualStudio2019;
    private GridLength _col2019Width;
    private GridLength _col2022Width;
    private GridLength _col2022PreWidth;
    private Solution? _selectedSolution;
    private bool _isAddSlnPressed;

    public const string PathEXE_VS2022Pre = @"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe";
    public const string PathEXE_VS2022 = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe";
    public const string PathEXE_VS2019 = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe";

    public ObservableCollection<Solution> Solutions
    {
        get => _solutions;
        set => SetField(ref _solutions, value);
    }

    public Solution SelectedSolution
    {
        get => _selectedSolution ?? new Solution();
        set => SetField(ref _selectedSolution, value);
    }

    public bool IsVS2022PreInstalled
    {
        get => _isVs2022PreInstalled;
        set
        {
            if (SetField(ref _isVs2022PreInstalled, value)) { OnPropertyChanged(nameof(Vs2022PreVisibility)); }
        }
    }

    public bool IsVS2022Installed
    {
        get => _isVs2022Installed;
        set
        {
            if (SetField(ref _isVs2022Installed, value)) { OnPropertyChanged(nameof(Vs2022Visibility)); }
        }
    }

    public bool IsVS2019Installed
    {
        get => _isVs2019Installed;
        set
        {
            if (SetField(ref _isVs2019Installed, value)) { OnPropertyChanged(nameof(Vs2019Visibility)); }
        }
    }

    public bool IsAdmin
    {
        get => _isAdmin;
        set => SetField(ref _isAdmin, value);
    }

    public bool IsAddSlnPressed
    {
        get => _isAddSlnPressed;
        set => SetField(ref _isAddSlnPressed, value);
    }

    public bool IsVisualStudio2022Pre
    {
        get => _isVisualStudio2022Pre;
        set
        {
            SetField(ref _isVisualStudio2022Pre, value);

            if (value)
            {
                IsVisualStudio2022 = false;
                IsVisualStudio2019 = false;

                UpdateColumnWidths();
            }
        }
    }

    public bool IsVisualStudio2022
    {
        get => _isVisualStudio2022;
        set
        {
            SetField(ref _isVisualStudio2022, value);

            if (value)
            {
                IsVisualStudio2022Pre = false;
                IsVisualStudio2019 = false;

                UpdateColumnWidths();
            }
        }
    }

    public bool IsVisualStudio2019
    {
        get => _isVisualStudio2019;
        set
        {
            SetField(ref _isVisualStudio2019, value);

            if (value)
            {
                IsVisualStudio2022Pre = false;
                IsVisualStudio2022 = false;

                UpdateColumnWidths();
            }
        }
    }

    public Visibility Vs2022PreVisibility => IsVS2022PreInstalled ? Visibility.Visible : Visibility.Collapsed;
    public Visibility Vs2022Visibility => IsVS2022Installed ? Visibility.Visible : Visibility.Collapsed;
    public Visibility Vs2019Visibility => IsVS2019Installed ? Visibility.Visible : Visibility.Collapsed;

    public GridLength Col2019Width
    {
        get => _col2019Width;
        set => SetField(ref _col2019Width, value);
    }

    public GridLength Col2022Width
    {
        get => _col2022Width;
        set => SetField(ref _col2022Width, value);
    }

    public GridLength Col2022PreWidth
    {
        get => _col2022PreWidth;
        set => SetField(ref _col2022PreWidth, value);
    }

    public MainViewModel()
    {
        IsVS2022PreInstalled = File.Exists(PathEXE_VS2022Pre);
        IsVS2022Installed = File.Exists(PathEXE_VS2022);
        IsVS2019Installed = File.Exists(PathEXE_VS2019);

        UpdateColumnWidths();

        var solutions = SolutionManager.GetSolutions();

        
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public void AddSolutions(bool isFolderPicker)
    {
        IsAddSlnPressed = false;

        foreach (var solution in SolutionManager.FindSolution(isFolderPicker))
        {
            Solutions.Add(solution);
        }

        SolutionManager.SaveSolutions(Solutions.ToList());
    }

    public bool AvviaVisualStudio()
    {
        var p = new Process();
        var st = new ProcessStartInfo
        {
            Arguments = $"\"{SelectedSolution.Path}\"",
            Verb = IsAdmin ? "runas" : ""
        };

        if (IsVisualStudio2022Pre)
        {
            st.FileName = PathEXE_VS2022Pre;
        }
        else if (IsVisualStudio2022)
        {
            st.FileName = PathEXE_VS2022;
        }
        else if (IsVisualStudio2019)
        {
            st.FileName = PathEXE_VS2019;
        }

        if (File.Exists(st.FileName))
        {
            p.StartInfo = st;
            p.Start();

            return true;
        }

        return false;
    }

    private void UpdateColumnWidths()
    {
        var visibleButtons = new List<Visibility>
        {
            Vs2019Visibility, Vs2022Visibility, Vs2022PreVisibility
        }.Count(v => v == Visibility.Visible);

        if (visibleButtons == 1)
        {
            if (Vs2019Visibility == Visibility.Visible)
            {
                Col2019Width = new GridLength(1, GridUnitType.Star);
                Col2022Width = new GridLength(0);
                Col2022PreWidth = new GridLength(0);
            }
            else if (Vs2022Visibility == Visibility.Visible)
            {
                Col2019Width = new GridLength(0);
                Col2022Width = new GridLength(1, GridUnitType.Star);
                Col2022PreWidth = new GridLength(0);
            }
            else
            {
                Col2019Width = new GridLength(0);
                Col2022Width = new GridLength(0);
                Col2022PreWidth = new GridLength(1, GridUnitType.Star);
            }
        }
        else if (visibleButtons == 2)
        {
            if (Vs2019Visibility == Visibility.Visible && Vs2022Visibility == Visibility.Visible)
            {
                Col2019Width = new GridLength(1, GridUnitType.Star);
                Col2022Width = new GridLength(1, GridUnitType.Star);
                Col2022PreWidth = new GridLength(0);
            }
            else if (Vs2019Visibility == Visibility.Visible && Vs2022PreVisibility == Visibility.Visible)
            {
                Col2019Width = new GridLength(1, GridUnitType.Star);
                Col2022Width = new GridLength(0);
                Col2022PreWidth = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                Col2019Width = new GridLength(0);
                Col2022Width = new GridLength(1, GridUnitType.Star);
                Col2022PreWidth = new GridLength(1, GridUnitType.Star);
            }
        }
        else
        {
            Col2019Width = new GridLength(1, GridUnitType.Star);
            Col2022Width = new GridLength(1, GridUnitType.Star);
            Col2022PreWidth = new GridLength(1, GridUnitType.Star);
        }

        OnPropertyChanged(nameof(Col2019Width));
        OnPropertyChanged(nameof(Col2022Width));
        OnPropertyChanged(nameof(Col2022PreWidth));
    }

    public void DeleteSolution()
    {

    }
}