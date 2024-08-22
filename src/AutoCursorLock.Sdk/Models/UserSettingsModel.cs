// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Represents the settings for a user.
/// </summary>
public record UserSettingsModel
{
    /// <summary>
    /// The hot key to use to toggle the cursor lock.
    /// </summary>
    public HotKeyModel? HotKey { get; init; }

    /// <summary>
    /// The app locks to use.
    /// </summary>
    [JsonRequired]
    required public AppLockSettingsModel[] AppLocks { get; init; }
}
