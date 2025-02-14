// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

using Serilog.Events;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the settings for a user.
/// </summary>
public record UserSettingsModel
{
    /// <summary>
    /// Gets the hot key to use to toggle the cursor lock.
    /// </summary>
    public HotKeyModel? HotKey { get; init; }

    /// <summary>
    /// Gets the log level.
    /// </summary>
    public LogEventLevel LogLevel { get; init; } = LogEventLevel.Information;

    /// <summary>
    /// Gets the app locks to use.
    /// </summary>
    [JsonRequired]
    required public AppLockSettingsModel[] AppLocks { get; init; }
}
