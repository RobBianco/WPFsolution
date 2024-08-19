using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using VisualStudioStarter.Business;
using VisualStudioStarter.ObjectModels;
using VisualStudioStarter.Utils;

namespace VisualStudioStarter.ViewModels;

public class SolutionPageViewModel : BaseViewModel
{
    private ObservableCollection<Solution> _solutions = [];
    private ObservableCollection<Solution> _pinnedSolutions = [];
    private bool _isVs2022PreInstalled;
    private bool _isVs2022Installed;
    private bool _isVs2019Installed;
    private bool _isAdmin;
    //private bool _isVisualStudio2022Pre;
    //private bool _isVisualStudio2022;
    //private bool _isVisualStudio2019;
    private GridLength _col2019Width;
    private GridLength _col2022Width;
    private GridLength _col2022PreWidth;
    private Solution? _selectedSolution;
    private int _pinnedRow;
    private int _unPinnedRow;

    public const string PathEXE_VS2022Pre =
        @"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe";

    public const string PathEXE_VS2022 =
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe";

    public const string PathEXE_VS2019 =
        @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe";

    public ObservableCollection<Solution> Solutions
    {
        get => _solutions;
        set => SetField(ref _solutions, value);
    }

    public ObservableCollection<Solution> PinnedSolutions
    {
        get => _pinnedSolutions;
        set => SetField(ref _pinnedSolutions, value);
    }

    public Solution? SelectedSolution
    {
        get => _selectedSolution ?? new Solution();
        set => SetField(ref _selectedSolution, value);
    }

    public bool IsPinnedVisible => PinnedSolutions.Any();
    public bool IsUnPinnedSolutonsVisible => Solutions.Any();
    public bool IsSolutonsVisible => IsPinnedVisible || IsUnPinnedSolutonsVisible;

    public bool IsVS2022PreInstalled
    {
        get => _isVs2022PreInstalled;
        set
        {
            if (SetField(ref _isVs2022PreInstalled, value))
            {
                OnPropertyChanged(nameof(Vs2022PreVisibility));
            }
        }
    }

    public bool IsVS2022Installed
    {
        get => _isVs2022Installed;
        set
        {
            if (SetField(ref _isVs2022Installed, value))
            {
                OnPropertyChanged(nameof(Vs2022Visibility));
            }
        }
    }

    public bool IsVS2019Installed
    {
        get => _isVs2019Installed;
        set
        {
            if (SetField(ref _isVs2019Installed, value))
            {
                OnPropertyChanged(nameof(Vs2019Visibility));
            }
        }
    }

    public bool IsAdmin
    {
        get => _isAdmin;
        set => SetField(ref _isAdmin, value);
    }


    //public bool IsVisualStudio2022Pre
    //{
    //    get => _isVisualStudio2022Pre;
    //    set
    //    {
    //        SetField(ref _isVisualStudio2022Pre, value);
    //        OnPropertyChanged(nameof(IsVisualStudioSelected));
    //        if (value)
    //        {
    //            IsVisualStudio2022 = false;
    //            IsVisualStudio2019 = false;

    //            UpdateColumnWidths();
    //        }
    //    }
    //}

    //public bool IsVisualStudio2022
    //{
    //    get => _isVisualStudio2022;
    //    set
    //    {
    //        SetField(ref _isVisualStudio2022, value);
    //        OnPropertyChanged(nameof(IsVisualStudioSelected));
    //        if (value)
    //        {
    //            IsVisualStudio2022Pre = false;
    //            IsVisualStudio2019 = false;

    //            UpdateColumnWidths();
    //        }
    //    }
    //}

    //public bool IsVisualStudio2019
    //{
    //    get => _isVisualStudio2019;
    //    set
    //    {
    //        SetField(ref _isVisualStudio2019, value);
    //        OnPropertyChanged(nameof(IsVisualStudioSelected));
    //        if (value)
    //        {
    //            IsVisualStudio2022Pre = false;
    //            IsVisualStudio2022 = false;

    //            UpdateColumnWidths();
    //        }
    //    }
    //}

    //public bool IsVisualStudioSelected => IsVisualStudio2019 || IsVisualStudio2022 || IsVisualStudio2022Pre;

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


    public int PinnedRow
    {
        get => _pinnedRow;
        set => SetField(ref _pinnedRow, value);
    }

    public int UnPinnedRow
    {
        get => _unPinnedRow;
        set => SetField(ref _unPinnedRow, value);
    }

    public SolutionPageViewModel()
    {
        VsStarterOptions.OnOptionsChanged += VsStarterOptionsOnOnOptionsChanged;

        IsVS2022PreInstalled = File.Exists(PathEXE_VS2022Pre);
        IsVS2022Installed = File.Exists(PathEXE_VS2022);
        IsVS2019Installed = File.Exists(PathEXE_VS2019);

        UpdateColumnWidths();

        Solutions.CollectionChanged += SolutionsOnCollectionChanged;
        PinnedSolutions.CollectionChanged += SolutionsOnCollectionChanged;

        AddSolutions(SolutionManager.GetSolutions());
    }

    private void VsStarterOptionsOnOnOptionsChanged(object? oldvalue, object? newvalue, string name)
    {
        switch (name)
        {
            case nameof(VsStarterOptions.PinnedPlacement):

                if (Enum.TryParse<PinnedPlacement>(newvalue?.ToString(), out var @new))
                {
                    switch (@new)
                    {
                        case PinnedPlacement.Top:
                            PinnedRow = 0;
                            UnPinnedRow = 1;
                            break;
                        case PinnedPlacement.Bottom:
                            PinnedRow = 1;
                            UnPinnedRow = 0;
                            break;
                    }
                }

                break;
        }
    }


    public event EventHandler? OnSolutionPinnedUnPinned;

    private void SolutionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsPinnedVisible));
        OnPropertyChanged(nameof(IsUnPinnedSolutonsVisible));
        OnPropertyChanged(nameof(IsSolutonsVisible));

    }

    public void AddSolutions(List<Solution> solutions)
    {
        foreach (var solution in solutions)
        {
            if (solution.IsPinned)
            {
                PinnedSolutions.Add(solution);
            }
            else
            {
                Solutions.Add(solution);
            }
        }

        OnSolutionPinnedUnPinned?.Invoke(this, EventArgs.Empty);

        SaveSolutions();
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
                Col2019Width = new(1, GridUnitType.Star);
                Col2022Width = new(0);
                Col2022PreWidth = new(0);
            }
            else if (Vs2022Visibility == Visibility.Visible)
            {
                Col2019Width = new(0);
                Col2022Width = new(1, GridUnitType.Star);
                Col2022PreWidth = new(0);
            }
            else
            {
                Col2019Width = new(0);
                Col2022Width = new(0);
                Col2022PreWidth = new(1, GridUnitType.Star);
            }
        }
        else if (visibleButtons == 2)
        {
            if (Vs2019Visibility == Visibility.Visible && Vs2022Visibility == Visibility.Visible)
            {
                Col2019Width = new(1, GridUnitType.Star);
                Col2022Width = new(1, GridUnitType.Star);
                Col2022PreWidth = new(0);
            }
            else if (Vs2019Visibility == Visibility.Visible && Vs2022PreVisibility == Visibility.Visible)
            {
                Col2019Width = new(1, GridUnitType.Star);
                Col2022Width = new(0);
                Col2022PreWidth = new(1, GridUnitType.Star);
            }
            else
            {
                Col2019Width = new(0);
                Col2022Width = new(1, GridUnitType.Star);
                Col2022PreWidth = new(1, GridUnitType.Star);
            }
        }
        else
        {
            Col2019Width = new(1, GridUnitType.Star);
            Col2022Width = new(1, GridUnitType.Star);
            Col2022PreWidth = new(1, GridUnitType.Star);
        }

        OnPropertyChanged(nameof(Col2019Width));
        OnPropertyChanged(nameof(Col2022Width));
        OnPropertyChanged(nameof(Col2022PreWidth));
    }

    public bool OpenSolutionProperties(Solution? sln = null)
    {
        sln ??= SelectedSolution;

        if (sln == null)
            return false;

        FilePropertiesUtils.ShowFileProperties(sln.Fileinfo?.FullName);
        return true;
    }

    public bool OpenSolutionDirectory(Solution? sln = null)
    {
        sln ??= SelectedSolution;

        if (sln == null)
            return false;

        if (!sln.DirectoryExist)
        {
            return false;
        }

        var argument = "/select, \"" + sln.Fileinfo?.FullName + "\"";
        Process.Start("explorer.exe", argument);
        return true;
    }

    public bool OpenSolution(Solution? sln = null)
    {
        sln ??= SelectedSolution;

        if (sln == null)
            return false;

        var p = new Process();
        var st = new ProcessStartInfo
        {
            Arguments = $"\"{sln.Path}\"",
            Verb = IsAdmin ? "runas" : ""
        };

        //if (IsVisualStudio2022Pre)
        //{
        //    st.FileName = PathEXE_VS2022Pre;
        //}
        //else if (IsVisualStudio2022)
        //{
        //    st.FileName = PathEXE_VS2022;
        //}
        //else if (IsVisualStudio2019)
        //{
        //    st.FileName = PathEXE_VS2019;
        //}

        if (File.Exists(st.FileName))
        {
            p.StartInfo = st;
            p.Start();

            return true;
        }

        return false;
    }

    public void Remove_Solution(Solution? sln = null)
    {
        sln ??= SelectedSolution;

        if (sln is null)
            return;

        if (sln.IsPinned)
        {
            PinnedSolutions.Remove(sln);
        }
        else
        {
            Solutions.Remove(sln);
        }

        OnSolutionPinnedUnPinned?.Invoke(this, EventArgs.Empty);

        SaveSolutions();
    }
    public void PinUnPin_Solution(Solution? sln = null)
    {
        sln ??= SelectedSolution;

        if (sln is null)
            return;

        sln.IsPinned = !sln.IsPinned;
        if (sln.IsPinned)
        {
            Solutions.Remove(sln);
            PinnedSolutions.Add(sln);
        }
        else
        {
            PinnedSolutions.Remove(sln);
            Solutions.Add(sln);
        }

        OnSolutionPinnedUnPinned?.Invoke(this, EventArgs.Empty);

        SaveSolutions();
    }

    public void SaveSolutions()
    {
        SolutionManager.SaveSolutions(PinnedSolutions, Solutions);
    }

    public void MoveDownSolution(Solution? sln = null)
    {
        sln ??= SelectedSolution;

        if (sln is null)
            return;

        if (sln.IsPinned)
        {
            int index = PinnedSolutions.IndexOf(sln);
            if (index >= 0 && index < PinnedSolutions.Count - 1)
            {
                PinnedSolutions.Move(index, index + 1);
            }
        }
        else
        {
            int index = Solutions.IndexOf(sln);
            if (index >= 0 && index < Solutions.Count - 1)
            {
                Solutions.Move(index, index + 1);
            }
        }

        SaveSolutions();
    }

    public void MoveUpSolution(Solution? sln = null)
    {
        sln ??= SelectedSolution;

        if (sln is null)
            return;

        if (sln.IsPinned)
        {
            int index = PinnedSolutions.IndexOf(sln);
            if (index > 0)
            {
                PinnedSolutions.Move(index, index - 1);
            }
        }
        else
        {
            int index = Solutions.IndexOf(sln);
            if (index > 0)
            {
                Solutions.Move(index, index - 1);
            }
        }

        SaveSolutions();
    }
}