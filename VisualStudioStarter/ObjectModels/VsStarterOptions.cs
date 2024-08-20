using System.Runtime.CompilerServices;
using VisualStudioStarter.Utils;
using VisualStudioStarter.ViewModels;

namespace VisualStudioStarter.ObjectModels;

public class VsStarterOptions : BaseViewModel
{
    #region FIELDS

    private PinnedPlacement? _pinnedPlacement;
    private StartPosition? _startPosition;
    private double? _width;
    private bool? _topMost;
    private VisualStudioVersion? _visualStudioSelected;

    #endregion

    #region PROPS

    public PinnedPlacement PinnedPlacement
    {
        get => _pinnedPlacement ?? Default.PinnedPlacement;
        set
        {
            var old = _pinnedPlacement;
            SetField(ref _pinnedPlacement, value);
            OnOptionsChanged?.Invoke(old, value);
        }
    }

    public StartPosition StartPosition
    {
        get => _startPosition ?? StartPosition.Center;
        set
        {
            var old = _startPosition;
            SetField(ref _startPosition, value);
            OnOptionsChanged?.Invoke(old, value);
        }
    }

    public VisualStudioVersion VisualStudioSelected
    {
        get => _visualStudioSelected ?? Default.VisualStudioSelected;
        set
        {
            var old = _visualStudioSelected;
            SetField(ref _visualStudioSelected, value);
            OnOptionsChanged?.Invoke(old, value);
        }
    }

    public double Width
    {
        get => _width ?? Default.Width;
        set
        {
            var old = _width;
            SetField(ref _width, value);
            OnOptionsChanged?.Invoke(old, value);
        }

    }

    public bool TopMost
    {
        get => _topMost ?? Default.TopMost;
        set
        {
            var old = _topMost;
            SetField(ref _topMost, value);
            OnOptionsChanged?.Invoke(old, value);
        }
    }

    public static VsStarterOptions Default => new()
    {
        Width = 500,
        TopMost = true,
        PinnedPlacement = PinnedPlacement.Top,
        VisualStudioSelected = VisualStudioVersion.None,
        StartPosition = StartPosition.Center,
    };

    #endregion

    #region EVENTS

    public static event VsStarterOptionsDelegate<object>? OnOptionsChanged;

    #endregion
}

public delegate void VsStarterOptionsDelegate<in T>(T? oldValue, T? newValue, [CallerMemberName] string name = "");