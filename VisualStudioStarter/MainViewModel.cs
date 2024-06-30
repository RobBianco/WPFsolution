using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using VisualStudioStarter.Utils;

namespace VisualStudioStarter;

public class MainViewModel : INotifyPropertyChanged
{
    private ObservableCollection<WorkSpace> _workSpace = [];
    private Boolean _isVs2022PreInstalled;
    private Boolean _isVs2022Installed;
    private Boolean _isVs2019Installed;
    private Boolean _isAdmin;
    private Boolean _isVisualStudio2022Pre;
    private Boolean _isVisualStudio2022;
    private Boolean _isVisualStudio2019;
    private Solution _selectenSolution;
    private GridLength _col2019Width;
    private GridLength _col2022Width;
    private GridLength _col2022PreWidth;
    private WorkSpace? _activeWorkSpace;
    private string? _comboBoxText;

    public WorkSpace? ActiveWorkSpace
    {
        get => _activeWorkSpace;
        set
        {
            var oldvalue = _activeWorkSpace;

            if (SetField(ref _activeWorkSpace, value))
            {
                OnActiveWorkSpaceChange(oldvalue);
            }
        }
    }

    private void OnActiveWorkSpaceChange(WorkSpace? oldvalue)
    {
        if (ActiveWorkSpace?.Type == WorkSpaceType.AddNew)
        {
            MessageBox.Show("aaaaaaaaaaaaaaaaaa");
        }
    }

    public const String PathEXE_VS2022Pre = @"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe";
    public const String PathEXE_VS2022 = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe";
    public const String PathEXE_VS2019 = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe";

    public ObservableCollection<WorkSpace> WorkSpaces
    {
        get => _workSpace;
        set
        {
            SetField(ref _workSpace, value);
            OnPropertyChanged(nameof(ActiveWorkSpace));
        }
    }

    public Solution SelectenSolution
    {
        get => _selectenSolution;
        set => SetField(ref _selectenSolution, value);
    }

    public Boolean IsVS2022PreInstalled
    {
        get => _isVs2022PreInstalled;
        set
        {
            if (SetField(ref _isVs2022PreInstalled, value)) { OnPropertyChanged(nameof(Vs2022PreVisibility)); }
        }
    }

    public Boolean IsVS2022Installed
    {
        get => _isVs2022Installed;
        set
        {
            if (SetField(ref _isVs2022Installed, value)) { OnPropertyChanged(nameof(Vs2022Visibility)); }
        }
    }

    public Boolean IsVS2019Installed
    {
        get => _isVs2019Installed;
        set
        {
            if (SetField(ref _isVs2019Installed, value)) { OnPropertyChanged(nameof(Vs2019Visibility)); }
        }
    }

    public Boolean IsAdmin
    {
        get => _isAdmin;
        set => SetField(ref _isAdmin, value);
    }

    public Boolean IsVisualStudio2022Pre
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

    public Boolean IsVisualStudio2022
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

    public Boolean IsVisualStudio2019
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

    public String? ComboBoxText
    {
        get => _comboBoxText;
        set => SetField(ref _comboBoxText, value);
    }

    public MainViewModel()
    {
        var ws = SolutionManager.GetWorkSpaces();

        foreach (var workSpace in ws)
        {
            WorkSpaces.Add(workSpace);
        }

        if (ActiveWorkSpace == null && WorkSpaces.Any())
        {
            WorkSpaces.First(x => x.Type != WorkSpaceType.AddNew).Active = true;
        }

        SetActiveWorkSpace();
        
        IsVS2022PreInstalled = File.Exists(PathEXE_VS2022Pre);
        IsVS2022Installed = File.Exists(PathEXE_VS2022);
        IsVS2019Installed = File.Exists(PathEXE_VS2019);

        UpdateColumnWidths();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

    protected Boolean SetField<T>(ref T field, T value, [CallerMemberName] String? propertyName = null)
    {
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public void OpenOrAddWorkSpace(MainWindow mainWindow)
    {
        if (ComboBoxText != null)
        {
            if (WorkSpaces.All(x => x.Path != ComboBoxText))
            {
                if (Directory.Exists(ComboBoxText))
                {
                    WorkSpaces.Add(new WorkSpace
                    {
                        Path = ComboBoxText,
                        Active = true,
                        Solutions = SolutionManager.GetSolutions(ComboBoxText)
                    });
                    SolutionManager.SaveWorkspaces(WorkSpaces.ToList());
                }
            }
        }

        if (WorkSpaces.Any(x => x.Path == ComboBoxText))
        {
            foreach (var workSpace in WorkSpaces) { workSpace.Active = false; }

            WorkSpaces.First(x => x.Path == ComboBoxText).Active = true;

            SetActiveWorkSpace();
        }
    }

    public void SetActiveWorkSpace()
    {
        if (WorkSpaces.Any(x => x.Active))
        {
            ActiveWorkSpace = WorkSpaces.First(x => x.Active);
        }

        OnPropertyChanged(nameof(ActiveWorkSpace));
    }

    public Boolean AvviaVisualStudio()
    {
        var p = new Process();
        var st = new ProcessStartInfo
        {
            Arguments = $"\"{SelectenSolution.Path}\"",
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

    public void DeleteWorkSpace()
    {
        if (WorkSpaces.Any(x => x.Path == ComboBoxText))
        {
            var w = WorkSpaces.First(x => x.Path == ComboBoxText);
            WorkSpaces.Remove(w);
        }
    }
}