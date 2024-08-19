using System.Reflection;

namespace VisualStudioStarter.ViewModels;

public class MainViewModel : BaseViewModel
{
    private bool _isOptionsDrawerOpen;

    public bool IsOptionsDrawerOpen
    {
        get => _isOptionsDrawerOpen;
        set => SetField(ref _isOptionsDrawerOpen, value);
    }

    public string Version => Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() ?? "error";
}