// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Models;

using AutoCursorLock.Native;
using AutoCursorLock.Sdk.Models;

/// <summary>
/// Extensions for <see cref="HotKey"/>.
/// </summary>
public static class HotKeyExtensions
{
    /// <summary>
    /// Converts a <see cref="HotKeyModel"/> to a <see cref="HotKey"/>.
    /// </summary>
    /// <param name="hotKey">The model.</param>
    /// <param name="id">The ID to give this hotkey.</param>
    /// <returns>The view model.</returns>
    public static HotKey ToViewModel(this HotKeyModel hotKey, int id)
    {
        return new HotKey(
            id,
            hotKey.Modifiers,
            hotKey.VirtualKey
        );
    }

    /// <summary>
    /// Converts a <see cref="HotKey"/> to a <see cref="HotKeyModel"/>.
    /// </summary>
    /// <param name="hotKey">The view model.</param>
    /// <returns>The model.</returns>
    public static HotKeyModel ToModel(this HotKey hotKey)
    {
        return new HotKeyModel
        {
            Modifiers = hotKey.ModifierKeys,
            VirtualKey = hotKey.Key,
        };
    }
}
