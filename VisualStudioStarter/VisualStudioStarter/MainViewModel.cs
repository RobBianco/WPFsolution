using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VisualStudioStarter
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private String _pathSolutions;
        private ObservableCollection<WorkSpace> _workSpace = [];
        private Boolean _isVs2022PreInstalled;
        private Boolean _isVs2022Installed;
        private Boolean _isVs2019Installed;
        private Boolean _isAdmin;
        private Boolean _isVisualStudio2022Pre;
        private Boolean _isVisualStudio2022;
        private Boolean _isVisualStudio2019;
        private Solution _selectenSolution;
        public WorkSpace? ActiveWorkSpace => WorkSpaces.ToList().Find(x => x.Active);

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

        public String PathSolutions
        {
            get => _pathSolutions;
            set
            {
                if(SetField(ref _pathSolutions, value))
                {
                }
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
                }
            }
        }

        public Visibility Vs2022PreVisibility => IsVS2022PreInstalled ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Vs2022Visibility => IsVS2022Installed ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Vs2019Visibility => IsVS2019Installed ? Visibility.Visible : Visibility.Collapsed;

        public MainViewModel()
        {
            var ws = SolutionManager.GetWorkSpaces();

            foreach (var workSpace in ws)
            {
                WorkSpaces.Add(workSpace);
            }

            if (ActiveWorkSpace == null && WorkSpaces.Any())
            {
                WorkSpaces.First().Active = true;
            }

            PathSolutions = ActiveWorkSpace?.Path ?? "";

            IsVS2022PreInstalled = File.Exists(PathEXE_VS2022Pre);
            IsVS2022Installed = File.Exists(PathEXE_VS2022);
            IsVS2019Installed = File.Exists(PathEXE_VS2019);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        protected Boolean SetField<T>(ref T field, T value, [CallerMemberName] String? propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void RefreshSolutions(MainWindow mainWindow)
        {
            try
            {
                if (WorkSpaces.Any(x => x.Path == _pathSolutions))
                {
                    foreach (var workSpace in WorkSpaces) { workSpace.Active = false; }

                    WorkSpaces.First(x => x.Path == PathSolutions).Active = true;
                    return;
                }

                if (Directory.Exists(_pathSolutions))
                {
                    if (MessageBox.Show(mainWindow, $"Salvare il workspace \r\n\r\n {_pathSolutions}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        WorkSpaces.Add(new WorkSpace
                        {
                            Path = _pathSolutions,
                            Active = true,
                            Solutions = SolutionManager.GetSolutions(_pathSolutions)
                        });
                        SolutionManager.SaveWorkspaces(WorkSpaces.ToList());
                    }

                    return;
                }
            }
            finally
            {
                    OnPropertyChanged(nameof(ActiveWorkSpace));
            }
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
    }
}
