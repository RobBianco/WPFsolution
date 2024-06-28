using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace VisualStudioStarter;

public static class DwmDropShadow
{
    private static Window? _win;

    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        public Int32 Left;
        public Int32 Right;
        public Int32 Top;
        public Int32 Bottom;
    }

    [DllImport("dwmapi.dll", PreserveSig = true)]
    public static extern Int32 DwmSetWindowAttribute(IntPtr hwnd, Int32 attr, ref Int32 attrValue, Int32 attrSize);

    [DllImport("dwmapi.dll")]
    public static extern Int32 DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

    /// <summary>
    /// Drops a standard shadow to a WPF Window, even if the window is borderless. Only works with DWM (Windows Vista or newer).
    /// This method is much more efficient than setting AllowsTransparency to true and using the DropShadow effect,
    /// as AllowsTransparency involves a huge performance issue (hardware acceleration is turned off for all the window).
    /// </summary>
    /// <param name="window">Window to which the shadow will be applied</param>
    public static void DropShadowToWindow(Window window)
    {
        _win = window;
        var source = PresentationSource.FromVisual(_win) as HwndSource;
        source?.AddHook(WndProc);
    }

    private static IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
    {
        if (_win != null)
        {
            var helper = new WindowInteropHelper(_win);
            var val = 2;
            var ret1 = DwmSetWindowAttribute(helper.Handle, 2, ref val, 4);

            if (ret1 == 0)
            {
                var m = new Margins { Bottom = 1, Left = 1, Right = 1, Top = 1 };
                DwmExtendFrameIntoClientArea(helper.Handle, ref m);
            }
        }

        return IntPtr.Zero;
    }
}