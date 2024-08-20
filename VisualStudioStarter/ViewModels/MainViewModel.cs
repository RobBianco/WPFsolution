using System.Reflection;

namespace VisualStudioStarter.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region FIELDS

    private bool _isOptionsDrawerOpen;

    #endregion

    #region PROPS

    public bool IsOptionsDrawerOpen
    {
        get => _isOptionsDrawerOpen;
        set => SetField(ref _isOptionsDrawerOpen, value);
    }

    public string Version => Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() ?? "error";

    #endregion
}