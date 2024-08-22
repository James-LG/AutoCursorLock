// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

using System;

/// <summary>
/// Handles the cursor clipping with the Windows API.
/// </summary>
public static class MouseHandler
{
    public static bool LockCursorToBorder(BorderDimensions border)
    {
        var rect = default(NativeMethods.NativeRectangle);
        rect.top = border.Top;
        rect.bottom = border.Bottom;
        rect.left = border.Left;
        rect.right = border.Right;

        return NativeMethods.ClipCursor(rect);
    }

    /// <summary>
    /// Removes the cursor clips and unlocks it for free movement.
    /// </summary>
    /// <returns>Whether the removal of the cursor clipping was a success.</returns>
    public static bool UnlockCursor()
    {
        return NativeMethods.ClipCursor(IntPtr.Zero);
    }
}
