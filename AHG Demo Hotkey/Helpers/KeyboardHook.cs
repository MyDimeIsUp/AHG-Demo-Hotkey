﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

// Original: https://stackoverflow.com/questions/1639331/using-global-keyboard-hook-wh-keyboard-ll-in-wpf-c-sharp
// Ported to be WPF-compatiable by My Dime Is Up (P.S.)

namespace AHG_Demo_Hotkey;

public class KeyboardHook {
    #region Constant, Structure and Delegate Definitions
    /// <summary>
    /// defines the callback type for the hook
    /// </summary>
    public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
    private keyboardHookProc _keyboardHookProc;

    public struct keyboardHookStruct {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    const int WH_KEYBOARD_LL = 13;
    const int WM_KEYDOWN = 0x100;
    const int WM_KEYUP = 0x101;
    const int WM_SYSKEYDOWN = 0x104;
    const int WM_SYSKEYUP = 0x105;
    #endregion

    #region Instance Variables
    /// <summary>
    /// The collections of keys to watch for
    /// </summary>
    public List<Key> HookedKeys = new List<Key>();
    /// <summary>
    /// Handle to the hook, need this to unhook and call the next hook
    /// </summary>
    IntPtr hhook = IntPtr.Zero;
    #endregion

    #region Events
    /// <summary>
    /// Occurs when one of the hooked keys is pressed
    /// </summary>
    public event KeyEventHandler KeyDown;
    /// <summary>
    /// Occurs when one of the hooked keys is released
    /// </summary>
    public event KeyEventHandler KeyUp;
    #endregion

    #region Constructors and Destructors
    /// <summary>
    /// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
    /// </summary>
    public KeyboardHook() {
        _keyboardHookProc = new keyboardHookProc(hookProc);
        hook();
    }

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="globalKeyboardHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
    /// </summary>
    ~KeyboardHook() {
        unhook();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Installs the global hook
    /// </summary>
    public void hook() {
        IntPtr hInstance = LoadLibrary("User32");
        this.hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardHookProc, hInstance, 0);
    }

    /// <summary>
    /// Uninstalls the global hook
    /// </summary>
    public void unhook() {
        UnhookWindowsHookEx(hhook);
    }

    /// <summary>
    /// The callback for the keyboard hook
    /// </summary>
    /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
    /// <param name="wParam">The event type</param>
    /// <param name="lParam">The keyhook event information</param>
    /// <returns></returns>
    public int hookProc(int code, int wParam, ref keyboardHookStruct lParam) {
        if (code >= 0) {
            if (HookedKeys.Count <= 0) return CallNextHookEx(hhook, code, wParam, ref lParam);

            Key KeyPressed = (Key)KeyInterop.KeyFromVirtualKey(lParam.vkCode); // Wee need this since lParam.vkCode uses Win32 Key. Convert to WPF key

            if (HookedKeys.Contains(KeyPressed)) {
                HwndSource DummySource = new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero); // This is needed because when we don't have focus, Keyboard.PrimaryDevice.ActiveSource returns null

                KeyEventArgs KeyEvent = new KeyEventArgs(Keyboard.PrimaryDevice, DummySource, 0, KeyPressed);
                KeyEvent.RoutedEvent = Keyboard.KeyDownEvent; // System.Windows.Input KeyEvent requires a RoutedEvent

                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null)) {
                    KeyDown(this, KeyEvent);
                }

                if (KeyEvent.Handled) {
                    return 1;
                }
            }
        }

        return CallNextHookEx(hhook, code, wParam, ref lParam);
    }
    #endregion

    #region DLL imports
    /// <summary>
    /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
    /// </summary>
    /// <param name="idHook">The id of the event you want to hook</param>
    /// <param name="callback">The callback.</param>
    /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
    /// <param name="threadId">The thread you want to attach the event to, can be null</param>
    /// <returns>a handle to the desired hook</returns>
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

    /// <summary>
    /// Unhooks the windows hook.
    /// </summary>
    /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
    /// <returns>True if successful, false otherwise</returns>
    [DllImport("user32.dll")]
    static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    /// <summary>
    /// Calls the next hook.
    /// </summary>
    /// <param name="idHook">The hook id</param>
    /// <param name="nCode">The hook code</param>
    /// <param name="wParam">The wparam.</param>
    /// <param name="lParam">The lparam.</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

    /// <summary>
    /// Loads the library.
    /// </summary>
    /// <param name="lpFileName">Name of the library</param>
    /// <returns>A handle to the library</returns>
    [DllImport("kernel32.dll")]
    static extern IntPtr LoadLibrary(string lpFileName);
    #endregion
}
