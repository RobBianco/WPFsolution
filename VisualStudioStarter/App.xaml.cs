using System.Windows;
using VisualStudioStarter.Business;

namespace VisualStudioStarter;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        OptionsManager.LoadOptions();

        base.OnStartup(e);
    }
}