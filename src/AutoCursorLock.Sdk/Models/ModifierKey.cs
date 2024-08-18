// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

/// <summary>
/// Represents a modifier key for a hot key.
/// </summary>
/// <remarks>
/// The values are Win32 virtual key codes.
/// </remarks>
public enum ModifierKey
{
    /// <summary>
    /// No modifier key.
    /// </summary>
    None = 0x0000,

    /// <summary>
    /// The "Alt" key.
    /// </summary>
    Alt = 0x0001,

    /// <summary>
    /// The "Ctrl" key.
    /// </summary>
    Control = 0x0002,

    /// <summary>
    /// The "Shift" key.
    /// </summary>
    Shift = 0x0004,

    /// <summary>
    /// The "Windows" key.
    /// </summary>
    Windows = 0x0008,
}
