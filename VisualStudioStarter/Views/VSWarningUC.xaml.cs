using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Timer = System.Timers.Timer;

namespace VisualStudioStarter.Views;

/// <summary>
/// Interaction logic for VSwarningUC.xaml
/// </summary>
public partial class VSWarningUC : INotifyPropertyChanged
{
    private double _progressValue;

    private readonly Timer timer = new();
    private double _maxProgressValue;
    private readonly double _totalDurationMs; // Durata totale in millisecondi

    public VSWarningUC(double? durationMs = null)
    {
        InitializeComponent();

        _totalDurationMs = durationMs ?? 2000; // Ad esempio, 10 secondi se non viene specificato
        MaxProgressValue = 100; // Ad esempio, se la barra rappresenta percentuali
        ProgressValue = 0;

        timer.Interval = 10;
        timer.Elapsed += TimerOnElapsed;
        timer.Start();
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            double increment = (MaxProgressValue / _totalDurationMs) * timer.Interval;
            ProgressValue += increment;

            // Ferma il timer quando il progresso raggiunge o supera il valore massimo
            if (ProgressValue >= MaxProgressValue)
            {
                ProgressValue = MaxProgressValue; // Assicurati che non superi il massimo
                timer.Stop();
                DialogHost.GetDialogSession("SolutionDialogHost")?.Close();
            }
        });
    }

    public double ProgressValue
    {
        get => _progressValue;
        set => SetField(ref _progressValue, value);
    }

    public double MaxProgressValue
    {
        get => _maxProgressValue;
        set => SetField(ref _maxProgressValue, value);
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

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        DialogHost.GetDialogSession("SolutionDialogHost")?.Close();
    }
}