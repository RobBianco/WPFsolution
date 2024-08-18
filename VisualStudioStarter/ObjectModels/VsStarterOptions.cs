using System.Runtime.CompilerServices;
using VisualStudioStarter.Business;
using VisualStudioStarter.Utils;
using VisualStudioStarter.ViewModels;

namespace VisualStudioStarter.ObjectModels;

public class VsStarterOptions : BaseViewModel
{
    private PinnedPlacement _pinnedPlacement;
    private double _width;
    private double _maxHeight;
    private bool _topMost;

    public PinnedPlacement PinnedPlacement
    {
        get => _pinnedPlacement;
        set
        {
            var old = _pinnedPlacement;
            if (SetField(ref _pinnedPlacement, value))
            {
                OnOptionsChanged?.Invoke(old, value);
            }
        }

    }

    public double Width
    {
        get => _width;
        set
        {
            var old = _width;
            if (SetField(ref _width, value))
            {
                OnOptionsChanged?.Invoke(old, value);
            }
        }

    }

    public double MaxHeight
    {
        get => _maxHeight;
        set
        {
            var old = _maxHeight;
            if (SetField(ref _maxHeight, value))
            {
                OnOptionsChanged?.Invoke(old, value);
            }
        }
    }

    public bool TopMost
    {
        get => _topMost;
        set
        {
            var old = _topMost;
            if (SetField(ref _topMost, value))
            {
                OnOptionsChanged?.Invoke(old, value);
            }
        }
    }

    public event VsStarterOptionsDelegate<object>? OnOptionsChanged;

    public static VsStarterOptions Default => new()
    {
        MaxHeight = 0,
        Width = 0,
        PinnedPlacement = PinnedPlacement.Top
    };
}

public delegate void VsStarterOptionsDelegate<in T>(T oldValue, T newValue, [CallerMemberName] string name = "");