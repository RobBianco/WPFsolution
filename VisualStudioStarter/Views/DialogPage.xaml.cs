using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using MaterialDesignThemes.Wpf;

namespace VisualStudioStarter.Views;

/// <summary>
/// Interaction logic for DialogPage.xaml
/// </summary>
public partial class DialogPage : INotifyPropertyChanged
{
    private string _solutionName;

    public string SolutionName
    {
        get => _solutionName;
        set => SetField(ref _solutionName, value);
    }

    public DialogPage(string solutionName)
    {
        InitializeComponent();
        SolutionName = solutionName;
    }

    private void BtnYes_OnClick(object sender, RoutedEventArgs e)
    {
        DialogHost.GetDialogSession("SolutionDialogHost")?.Close(DialogResult.Yes);
    }

    private void BtnNo_OnClick(object sender, RoutedEventArgs e)
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