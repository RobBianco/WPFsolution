using System.Windows;
using VisualStudioStarter.Business;
using VisualStudioStarter.ObjectModels;

namespace VisualStudioStarter;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public VsStarterOptions? Options { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        Options = OptionsManager.GetOptions();

        base.OnStartup(e);
    }
}