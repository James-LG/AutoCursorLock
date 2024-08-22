// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

/// <summary>
/// Represents the settings for an app lock.
/// </summary>
/// <param name="Name">The name of the process.</param>
/// <param name="Path">The path to the application.</param>
/// <param name="Type">The type of app lock to use.</param>
public record AppLockSettingsModel(
    string Name,
    string Path,
    AppLockType Type
);
