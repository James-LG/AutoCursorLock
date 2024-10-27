// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

/// <summary>
/// Represents the settings for an app lock.
/// </summary>
/// <param name="Name">The name of the process.</param>
/// <param name="Path">The path to the application.</param>
/// <param name="Type">The type of app lock to use.</param>
/// <param name="Margin">The margin around the window for the app lock.</param>
public record AppLockSettingsModel(
    string Name,
    string? Path,
    AppLockType Type,
    AppLockMargin? Margin
)
{
    /// <summary>
    /// Creates an <see cref="AppLockSettingsModel"/> from the name and path, using defaults for other properties.
    /// </summary>
    /// <param name="name">The name of the app.</param>
    /// <param name="path">The path to the app.</param>
    /// <returns>The model.</returns>
    public static AppLockSettingsModel FromNameAndPath(string name, string? path)
    {
        return new AppLockSettingsModel(name, path, AppLockType.Window, new AppLockMargin(0, 0, 0, 0));
    }
}
