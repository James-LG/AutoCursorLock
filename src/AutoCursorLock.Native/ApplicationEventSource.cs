// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

using System;
using System.Diagnostics;

/// <summary>
/// Handles the detection of application switching and exposes the corresponding events.
/// </summary>
public class ApplicationEventSource
{
    /// <remarks>
    /// This has to be an instance member so it doesn't get garbage collected.
    /// </remarks>
    private NativeMethods.WinEventDelegate? eventDelegate = null;

    private IntPtr hook;

    /// <summary>
    /// Raised when the application in the foreground has changed.
    /// </summary>
    public event EventHandler<ApplicationEventArgs>? ApplicationChanged;

    /// <summary>
    /// Registers the application monitor with the Windows API.
    /// </summary>
    /// <returns>Success indicator.</returns>
    public bool Register()
    {
        this.eventDelegate = new NativeMethods.WinEventDelegate(WinEventProc);
        this.hook = NativeMethods.SetWinEventHook(
            NativeMethods.EVENT_SYSTEM_FOREGROUND,
            NativeMethods.EVENT_SYSTEM_FOREGROUND,
            IntPtr.Zero,
            this.eventDelegate,
            0,
            0,
            NativeMethods.WINEVENT_OUTOFCONTEXT);

        return this.hook != IntPtr.Zero;
    }

    /// <summary>
    /// Registers the application monitor with the Windows API.
    /// </summary>
    /// <returns>Success indicator.</returns>
    public bool Unregister()
    {
        return NativeMethods.UnhookWinEvent(this.hook);
    }

    /// <summary>
    /// Force an <see cref="ApplicationChanged"/> event with the current foreground window.
    /// </summary>
    public void Update()
    {
        var hwnd = NativeMethods.GetForegroundWindow();

        RaiseApplicationChanged(hwnd);
    }

    private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        RaiseApplicationChanged(hwnd);
    }

    private void RaiseApplicationChanged(IntPtr hwnd)
    {
        _ = NativeMethods.GetWindowThreadProcessId(hwnd, out uint processId);

        try
        {
            using Process p = Process.GetProcessById((int)processId);

            ApplicationChanged?.Invoke(this, new ApplicationEventArgs(hwnd, processId, p.ProcessName));
        }
        catch (InvalidOperationException)
        {
            // The process property is not defined because the process has exited or it does not have an identifier.
        }
        catch (ArgumentException)
        {
            // The process specified by the processId parameter is not running.
        }
    }
}
