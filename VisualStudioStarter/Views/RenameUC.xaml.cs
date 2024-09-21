using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;

namespace VisualStudioStarter.Views
{
    /// <summary>
    /// Interaction logic for RenameUC.xaml
    /// </summary>
    public partial class RenameUC : INotifyPropertyChanged
    {
        private string? _title;
        private readonly string _resetName;
        private string _newName;

        public string Title
        {
            get => _title ?? "Rename solution";
            set => SetField(ref _title, value);
        }

        public string NewName
        {
            get => _newName;
            set
            {
                if (SetField(ref _newName, value)) 
                    OnPropertyChanged(nameof(ResetEnable));
            }
        }

        public bool ResetEnable => _newName != _resetName;

        public RenameUC(string title, string name, string resetName)
        {
            _resetName = resetName;

            InitializeComponent();
            Title = title;
            NewName = name;
            
            txtName.SelectAll();
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            DialogHost.GetDialogSession("SolutionDialogHost")?.Close(DialogResult.Yes);
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogHost.GetDialogSession("SolutionDialogHost")?.Close(DialogResult.No);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            NewName = _resetName;
        }

    }
}
