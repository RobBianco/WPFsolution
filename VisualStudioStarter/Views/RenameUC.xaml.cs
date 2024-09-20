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
    private string? _name;

    public string Title
    {
        get => _title ?? "Rename solution";
        set => SetField(ref _title, value);
    }

    public new string Name
    {
        get => _name ?? "";
        set => SetField(ref _name, value);
    }

    public RenameUC(string title, string name)
    {
        InitializeComponent();
        Title = title;
        Name = name;
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
    }
}
