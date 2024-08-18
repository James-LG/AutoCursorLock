// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

using AutoCursorLock.Sdk.Models;
using System;

/// <summary>
/// Handles the registration of hot keys with Windows.
/// </summary>
public static class KeyHandler
{
    /// <summary>
    /// Register the given hot key with Windows. Windows must send the hot key events to a window
    /// so pointed to the given window handle.
    /// </summary>
    /// <param name="hotKey">The hot key to register.</param>
    /// <param name="hWnd">The handle of the window to send the hot key events to.</param>
    /// <returns>Whether the hot key was successfully registered.</returns>
    public static bool Register(HotKey hotKey, IntPtr hWnd)
    {
        var modifier = ModifierKey.None;
        foreach (var modifierKey in hotKey.ModifierKeys)
        {
            modifier |= modifierKey;
        }

        var modifierValue = (uint)modifier;

        return NativeMethods.RegisterHotKey(hWnd, hotKey.Id, modifierValue, (uint)hotKey.Key);
    }

    /// <summary>
    /// Unregister the given hot key with Windows. Windows must send the hot key events to a window
    /// so pointed to the given window handle.
    /// </summary>
    /// <param name="hotKey">The hot key to register.</param>
    /// <param name="hWnd">The handle of the window to send the hot key events to.</param>
    /// <returns>Whether the hot key was successfully unregistered.</returns>
    public static bool Unregister(HotKey hotKey, IntPtr hWnd)
    {
        return NativeMethods.UnregisterHotKey(hWnd, hotKey.Id);
    }
}
