// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

using AutoCursorLock.Sdk.Models;

/// <summary>
/// Represents a hot key combination.
/// </summary>
public class HotKey
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HotKey"/> class.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="modifierKeys">The modifier keys.</param>
    /// <param name="key">The key to be pressed.</param>
    public HotKey(int id, ModifierKey[] modifierKeys, int key)
    {
        Id = id;
        ModifierKeys = modifierKeys;
        Key = key;
    }

    /// <summary>
    /// Gets the ID of the HotKey.
    /// </summary>
    /// <remarks>
    /// This is used to register and unregister the hot key with Windows.
    /// </remarks>
    public int Id { get; }

    /// <summary>
    /// Gets the Win32 virtual key to be pressed.
    /// </summary>
    public int Key { get; }

    /// <summary>
    /// Gets the modifier keys.
    /// </summary>
    /// <remarks>
    /// This can be keys such as Shift, or Control.
    /// </remarks>
    public ModifierKey[] ModifierKeys { get; }
}
