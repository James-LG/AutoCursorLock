// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a hot key.
/// </summary>
public record HotKeyModel
{
    /// <summary>
    /// THe modifiers for the hot key.
    /// </summary>
    [JsonRequired]
    required public ModifierKey[] Modifiers { get; init; }

    /// <summary>
    /// The virtual key code for the hot key.
    /// </summary>
    [JsonRequired]
    required public int VirtualKey { get; init; }
}
