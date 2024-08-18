using System.Windows;
using System.Windows.Controls;

namespace VisualStudioStarter.Views;

/// <summary>
/// Logica di interazione per OptionsPage.xaml
/// </summary>
public partial class OptionsPage : Page
{
    public OptionsPage()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        
    }
}