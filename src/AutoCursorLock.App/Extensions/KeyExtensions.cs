// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Extensions;

using System.Windows.Input;

/// <summary>
/// Extension methods for the <see cref="Key"/> enumeration.
/// </summary>
internal static class KeyExtensions
{
    /// <summary>
    /// Converts a <see cref="Key"/> to a Win32 virtual key code.
    /// </summary>
    /// <param name="key">The key to convert.</param>
    /// <returns>The Win32 virtual key code.</returns>
    public static uint ToVirtualKey(this Key key)
    {
        return (uint)KeyInterop.VirtualKeyFromKey(key);
    }
}
