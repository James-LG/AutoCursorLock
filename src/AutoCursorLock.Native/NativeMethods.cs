// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Exposes Windows API functions as well as defining some Windows constants.
/// </summary>
public static class NativeMethods
{
    public const uint WINEVENT_OUTOFCONTEXT = 0;
    public const uint EVENT_SYSTEM_FOREGROUND = 3;

    /// <summary>
    /// Windows message ID for HotKeys.
    /// </summary>
    public const int WM_HOTKEY_MSG_ID = 0x0312;

    public const int MONITOR_DEFAULTTOPRIMERTY = 0x00000001;
    public const int MONITOR_DEFAULTTONEAREST = 0x00000002;

    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    public enum GetWindowLongIndex : int
    {
        GWL_WNDPROC = -4,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20,
        GWL_USERDATA = -21,
        GWL_ID = -12,
    }

    [Flags]
    public enum WindowStyles : int
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = -2147483648,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
    }

    public enum SystemMetric : int
    {
        SM_CXBORDER = 5,
        SM_CYBORDER = 6,
        SM_CXSIZEFRAME = 32,
        SM_CYSIZEFRAME = 33,
        SM_CYCAPTION = 4,
        SM_CXFIXEDFRAME = 7,
        SM_CYFIXEDFRAME = 8,
    }

    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    [DllImport("user32.dll")]
    public static extern bool UnhookWinEvent(IntPtr eventHook);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool ClipCursor(IntPtr rect);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool ClipCursor(NativeRectangle rect);

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hWnd, out NativeRectangle rect);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern WindowStyles GetWindowLong(IntPtr hwnd, GetWindowLongIndex index);

    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(SystemMetric index);

    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromWindow(IntPtr hwnd, int flags);

    [DllImport("user32.dll")]
    public static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out]NativeMonitorInfo lpmi);

    public struct NativeRectangle
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public sealed class NativeMonitorInfo
    {
        public int Size = Marshal.SizeOf(typeof(NativeMonitorInfo));
        public NativeRectangle Monitor;
        public NativeRectangle Work;
        public int Flags;
    }
}
