using VisualStudioStarter.Utils;

namespace VisualStudioStarter.ObjectModels;

public class VsStarterOptions
{

    public PinnedPlacement PinnedPlacement { get; set; }

    public double Width { get; set; }

    public double MaxHeight { get; set; }

    public static VsStarterOptions Default => new()
    {
        MaxHeight = 0,
        Width = 0,
        PinnedPlacement = PinnedPlacement.Top
    };
}