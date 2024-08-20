using System.Diagnostics;
using System.Runtime.InteropServices;

public class GlobalKeyboardHook : IDisposable
{
    // Definizione delle costanti e delle API esterne

    #region CONST

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;

    #endregion

    #region FIELDS

    private LowLevelKeyboardProc _proc;
    private IntPtr _hookID = IntPtr.Zero;

    #endregion

    #region CTOR

    public GlobalKeyboardHook()
    {
        _proc = HookCallback;
        _hookID = SetHook(_proc);
    }

    #endregion

    #region EVENTS

    public event EventHandler<KeyPressedEventArgs> KeyPressed;

    #endregion

    #region METHODS

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            KeyPressed?.Invoke(this, new KeyPressedEventArgs(vkCode));
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    public void Dispose()
    {
        UnhookWindowsHookEx(_hookID);
    }

    #endregion
}

public class KeyPressedEventArgs(int virtualKeyCode) : EventArgs
{
    public int VirtualKeyCode { get; private set; } = virtualKeyCode;
}
